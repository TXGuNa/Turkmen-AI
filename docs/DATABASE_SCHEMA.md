# Veritabanı Şeması — Turkmen AI

SQL Server 2022 üzerinde. Migration'lar EF Core ile yönetilecek.

## Tablolar

### Users
Kullanıcı hesapları.
```sql
CREATE TABLE Users (
    Id              UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Email           NVARCHAR(256) NOT NULL UNIQUE,
    PhoneNumber     NVARCHAR(32) NULL,
    PasswordHash    NVARCHAR(512) NOT NULL,
    FullName        NVARCHAR(200) NULL,
    PreferredLanguage NVARCHAR(8) NOT NULL DEFAULT 'tk',  -- tk, tr, ru, en
    CreatedAt       DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    LastLoginAt     DATETIME2 NULL,
    IsActive        BIT NOT NULL DEFAULT 1
);
```

### Subscriptions
Abonelik bilgileri.
```sql
CREATE TABLE Subscriptions (
    Id              UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId          UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Users(Id),
    PlanType        NVARCHAR(32) NOT NULL,  -- free, individual, premium, business
    StartedAt       DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    ExpiresAt       DATETIME2 NULL,
    IsActive        BIT NOT NULL DEFAULT 1,
    DailyQuestionLimit INT NOT NULL DEFAULT 5
);
```

### Conversations
Bir konuşma oturumu.
```sql
CREATE TABLE Conversations (
    Id              UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId          UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Users(Id),
    Module          NVARCHAR(32) NOT NULL,  -- language, accounting, law, banking
    Title           NVARCHAR(256) NULL,
    CreatedAt       DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt       DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
CREATE INDEX IX_Conversations_UserId ON Conversations(UserId);
```

### Messages
Konuşma mesajları.
```sql
CREATE TABLE Messages (
    Id              UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ConversationId  UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Conversations(Id),
    Role            NVARCHAR(16) NOT NULL,  -- user, assistant, system
    Content         NVARCHAR(MAX) NOT NULL,
    TokensUsed      INT NULL,
    CreatedAt       DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
CREATE INDEX IX_Messages_ConversationId ON Messages(ConversationId);
```

### KnowledgeDocuments
RAG için bilgi tabanı dökümanları (modül bazlı).
```sql
CREATE TABLE KnowledgeDocuments (
    Id              UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Module          NVARCHAR(32) NOT NULL,  -- language, accounting, law, banking
    SourceName      NVARCHAR(256) NOT NULL,  -- "Türkmen Vergi Kanunu m.45" vb.
    Title           NVARCHAR(512) NULL,
    Language        NVARCHAR(8) NOT NULL DEFAULT 'tk',
    CreatedAt       DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    IsPublic        BIT NOT NULL DEFAULT 1
);
CREATE INDEX IX_KnowledgeDocuments_Module ON KnowledgeDocuments(Module);
```

### DocumentChunks
RAG için döküman parçaları (chunklar) ve embedding'leri.
```sql
CREATE TABLE DocumentChunks (
    Id              UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    DocumentId      UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES KnowledgeDocuments(Id),
    ChunkIndex      INT NOT NULL,
    Content         NVARCHAR(MAX) NOT NULL,
    Embedding       VARBINARY(MAX) NOT NULL,  -- float[] olarak serialize edilmiş
    TokenCount      INT NOT NULL,
    CreatedAt       DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
CREATE INDEX IX_DocumentChunks_DocumentId ON DocumentChunks(DocumentId);
```

> Not: SQL Server 2025+ veya Azure SQL'de native VECTOR(N) datatype geliyor.
> O zaman bu kolon `Embedding VECTOR(1024)` olarak değişir, cosine similarity native olur.

### UsageEvents
Faturalama ve analitik için her kullanım kaydedilir.
```sql
CREATE TABLE UsageEvents (
    Id              BIGINT IDENTITY PRIMARY KEY,
    UserId          UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Users(Id),
    EventType       NVARCHAR(32) NOT NULL,  -- question, login, subscription_change
    Module          NVARCHAR(32) NULL,
    TokensUsed      INT NULL,
    ProviderUsed    NVARCHAR(64) NULL,  -- groq, openai, local-llama vb.
    CostMicroUsd    INT NULL,  -- maliyet takibi
    CreatedAt       DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
CREATE INDEX IX_UsageEvents_UserId_CreatedAt ON UsageEvents(UserId, CreatedAt);
```

## Şema İlişkileri

```
Users ──┬── Subscriptions
        ├── Conversations ── Messages
        └── UsageEvents

KnowledgeDocuments ── DocumentChunks  (RAG)
```

## Migration Stratejisi

EF Core Code-First kullanacağız. C# entity class'larından migration üretilir:

```bash
dotnet ef migrations add InitialCreate --project TurkmenAI.Infrastructure --startup-project TurkmenAI.Api
dotnet ef database update --project TurkmenAI.Infrastructure --startup-project TurkmenAI.Api
```

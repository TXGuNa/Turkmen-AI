"use client";
import { useState } from "react";

type Module = "language" | "accounting" | "law" | "banking";

const MODULES: { id: Module; tk: string; tr: string; icon: string }[] = [
  { id: "language", tk: "Dil we edebiýat", tr: "Dil ve Edebiyat", icon: "📚" },
  { id: "accounting", tk: "Buhgalter hasaby", tr: "Muhasebe", icon: "🧮" },
  { id: "law", tk: "Hukuk we ygtyýarnama", tr: "Hukuk ve Lisans", icon: "⚖️" },
  { id: "banking", tk: "Bank ulgamy", tr: "Bankacılık", icon: "🏦" }
];

interface ChatMessage {
  role: "user" | "assistant";
  content: string;
  sources?: string[];
}

export default function Home() {
  const [module, setModule] = useState<Module>("language");
  const [messages, setMessages] = useState<ChatMessage[]>([]);
  const [input, setInput] = useState("");
  const [loading, setLoading] = useState(false);

  async function send() {
    if (!input.trim()) return;
    const userMsg: ChatMessage = { role: "user", content: input };
    const next = [...messages, userMsg];
    setMessages(next);
    setInput("");
    setLoading(true);

    try {
      const res = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/api/assistant/ask`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          module,
          question: userMsg.content,
          history: messages.map((m) => ({ role: m.role, content: m.content }))
        })
      });
      const data = await res.json();
      setMessages([
        ...next,
        { role: "assistant", content: data.answer, sources: data.sources }
      ]);
    } catch (err) {
      setMessages([
        ...next,
        { role: "assistant", content: "Ýalňyşlyk boldy. Backend işleýärmi?" }
      ]);
    } finally {
      setLoading(false);
    }
  }

  return (
    <main className="mx-auto flex h-screen max-w-4xl flex-col p-4">
      <header className="mb-4 flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-turkmen-green">Türkmen AI</h1>
          <p className="text-sm text-neutral-600">
            Türkmen dilinde kömekçi: dil, buhgalter, hukuk, bank
          </p>
        </div>
      </header>

      <div className="mb-4 flex gap-2 overflow-x-auto">
        {MODULES.map((m) => (
          <button
            key={m.id}
            onClick={() => setModule(m.id)}
            className={`whitespace-nowrap rounded-full border px-4 py-2 text-sm transition ${
              module === m.id
                ? "border-turkmen-green bg-turkmen-green text-white"
                : "border-neutral-300 bg-white hover:border-turkmen-green"
            }`}
          >
            {m.icon} {m.tk}
          </button>
        ))}
      </div>

      <div className="flex-1 overflow-y-auto rounded-xl border border-neutral-200 bg-white p-4">
        {messages.length === 0 && (
          <div className="flex h-full flex-col items-center justify-center text-center text-neutral-400">
            <p className="text-lg">Salam! Soraňyzy ýazyň.</p>
            <p className="mt-2 text-sm">
              Saýlanan modul: <strong>{MODULES.find((m) => m.id === module)?.tk}</strong>
            </p>
          </div>
        )}
        {messages.map((m, i) => (
          <div
            key={i}
            className={`mb-3 flex ${m.role === "user" ? "justify-end" : "justify-start"}`}
          >
            <div
              className={`max-w-[80%] whitespace-pre-wrap rounded-2xl px-4 py-2 ${
                m.role === "user"
                  ? "bg-turkmen-green text-white"
                  : "bg-neutral-100 text-neutral-900"
              }`}
            >
              {m.content}
              {m.sources && m.sources.length > 0 && (
                <div className="mt-2 border-t border-neutral-300 pt-2 text-xs opacity-75">
                  Çeşmeler: {m.sources.join(", ")}
                </div>
              )}
            </div>
          </div>
        ))}
        {loading && <div className="text-neutral-400">Pikirlenýär...</div>}
      </div>

      <div className="mt-4 flex gap-2">
        <input
          value={input}
          onChange={(e) => setInput(e.target.value)}
          onKeyDown={(e) => e.key === "Enter" && send()}
          placeholder="Soraňyzy ýazyň..."
          className="flex-1 rounded-xl border border-neutral-300 px-4 py-3 focus:border-turkmen-green focus:outline-none"
        />
        <button
          onClick={send}
          disabled={loading}
          className="rounded-xl bg-turkmen-green px-6 py-3 font-medium text-white hover:opacity-90 disabled:opacity-50"
        >
          Iber
        </button>
      </div>
    </main>
  );
}

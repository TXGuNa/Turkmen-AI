/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{ts,tsx}"],
  theme: {
    extend: {
      colors: {
        turkmen: {
          green: "#00853e",
          red: "#c41e3a",
          gold: "#f5c518"
        }
      }
    }
  },
  plugins: []
};

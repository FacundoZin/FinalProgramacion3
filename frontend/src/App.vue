
<template>
  <div class="app-shell">
    <nav class="navbar">
      <div class="brand">
        <span class="logo-dot"></span>
        <div>
          <p class="brand-title">Crypto Portfolio</p>
          <p class="brand-subtitle">Seguimiento de cartera en ARS</p>
        </div>
      </div>
      <ul class="nav-links">
        <li><RouterLink to="/clients">Clientes</RouterLink></li>
        <li><RouterLink to="/transactions">Nueva transacción</RouterLink></li>
        <li><RouterLink to="/history">Historial</RouterLink></li>
        <li><RouterLink to="/analysis">Análisis</RouterLink></li>
        <li><RouterLink to="/best-exchange">¿Dónde comprar/vender?</RouterLink></li>
      </ul>
    </nav>
    <main class="container">
      <RouterView />
    </main>
  </div>
</template>

<script setup>
</script>

<style>
:root {
  color-scheme: light;
  --bg-dark: #0f172a;
  --bg-light: #f5f7fb;
  --primary: #6366f1;
  --primary-dark: #4f46e5;
  --accent: #22d3ee;
  --text: #0f172a;
  --muted: #64748b;
  --card: #ffffff;
  --border: #e2e8f0;
}

*,
*::before,
*::after {
  box-sizing: border-box;
}

body {
  margin: 0;
  font-family: 'Inter', 'Segoe UI', system-ui, -apple-system, BlinkMacSystemFont, sans-serif;
  background: radial-gradient(circle at top, rgba(148, 163, 255, 0.35), transparent 50%),
    var(--bg-light);
  color: var(--text);
}

.app-shell {
  min-height: 100vh;
  background: linear-gradient(180deg, rgba(15, 23, 42, 0.05), rgba(99, 102, 241, 0.05));
}

.navbar {
  position: sticky;
  top: 0;
  z-index: 10;
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem 2rem;
  background: rgba(15, 23, 42, 0.9);
  backdrop-filter: blur(8px);
  color: #f8fafc;
  border-bottom: 1px solid rgba(255, 255, 255, 0.08);
  box-shadow: 0 10px 30px rgba(15, 23, 42, 0.35);
}

.brand {
  display: flex;
  align-items: center;
  gap: 0.8rem;
}

.logo-dot {
  width: 40px;
  height: 40px;
  border-radius: 12px;
  background: linear-gradient(135deg, var(--accent), var(--primary));
  box-shadow: 0 10px 20px rgba(34, 211, 238, 0.35);
}

.brand-title {
  margin: 0;
  font-size: 1.05rem;
  font-weight: 600;
}

.brand-subtitle {
  margin: 0;
  font-size: 0.8rem;
  color: #cbd5f5;
}

.nav-links {
  list-style: none;
  display: flex;
  gap: 0.5rem;
  margin: 0;
  padding: 0;
}

.nav-links a {
  color: #e2e8f0;
  text-decoration: none;
  font-weight: 500;
  padding: 0.45rem 0.9rem;
  border-radius: 999px;
  transition: all 0.2s ease;
}

.nav-links a:hover {
  color: #fff;
  background: rgba(255, 255, 255, 0.12);
}

.nav-links a.router-link-active {
  background: linear-gradient(120deg, var(--primary), var(--accent));
  color: #fff;
  box-shadow: 0 8px 20px rgba(99, 102, 241, 0.4);
}

.container {
  width: min(1100px, 100%);
  margin: 0 auto;
  padding: clamp(1rem, 4vw, 3rem);
}

section {
  background: var(--card);
  border-radius: 1.25rem;
  padding: clamp(1.5rem, 3vw, 2.5rem);
  box-shadow: 0 25px 60px rgba(15, 23, 42, 0.08);
  border: 1px solid var(--border);
}

section + section {
  margin-top: 2rem;
}

section h2 {
  margin-top: 0;
  margin-bottom: 0.25rem;
  font-size: clamp(1.4rem, 2.4vw, 1.8rem);
  color: var(--text);
}

section p {
  color: var(--muted);
  margin-bottom: 1.5rem;
}

form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  max-width: 520px;
}

label {
  font-weight: 600;
  color: var(--text);
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
}

input,
select,
textarea {
  padding: 0.9rem 1rem;
  font-size: 1rem;
  border-radius: 0.8rem;
  border: 1px solid var(--border);
  background: #f8fafc;
  transition: border 0.2s ease, box-shadow 0.2s ease;
}

input:focus,
select:focus,
textarea:focus {
  outline: none;
  border-color: var(--primary);
  box-shadow: 0 0 0 3px rgba(99, 102, 241, 0.15);
}

button {
  cursor: pointer;
  border: none;
  border-radius: 0.9rem;
  padding: 0.85rem 1.25rem;
  font-size: 1rem;
  font-weight: 600;
  color: #fff;
  background: linear-gradient(120deg, var(--primary), var(--accent));
  box-shadow: 0 15px 30px rgba(99, 102, 241, 0.35);
  transition: transform 0.2s ease, box-shadow 0.2s ease;
}

button:hover {
  transform: translateY(-1px);
  box-shadow: 0 20px 35px rgba(99, 102, 241, 0.4);
}

button.secondary {
  background: #e2e8f0;
  color: var(--text);
  box-shadow: none;
}

table {
  border-collapse: collapse;
  width: 100%;
  margin-top: 1.5rem;
  border: 1px solid var(--border);
  border-radius: 1rem;
  overflow: hidden;
  font-size: 0.95rem;
}

thead {
  background: #f8fafc;
}

th,
td {
  padding: 0.9rem 1rem;
  text-align: left;
  border-bottom: 1px solid var(--border);
}

tbody tr:hover {
  background: rgba(99, 102, 241, 0.05);
}

.actions button {
  margin-right: 0.35rem;
}

.error,
.success {
  padding: 0.85rem 1rem;
  border-radius: 0.75rem;
  font-weight: 500;
  margin-bottom: 1rem;
}

.error {
  color: #991b1b;
  background: #fee2e2;
  border: 1px solid #fecaca;
}

.success {
  color: #166534;
  background: #dcfce7;
  border: 1px solid #bbf7d0;
}

@media (max-width: 900px) {
  .navbar {
    flex-direction: column;
    align-items: flex-start;
    gap: 0.75rem;
  }

  .nav-links {
    flex-wrap: wrap;
  }

  form {
    max-width: 100%;
  }
}
</style>

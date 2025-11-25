<template>
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
      <li>
        <RouterLink to="/best-exchange">¿Dónde comprar/vender?</RouterLink>
      </li>
    </ul>
  </nav>
</template>

<script setup></script>

<style scoped>
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

@media (max-width: 900px) {
  .navbar {
    flex-direction: column;
    align-items: flex-start;
    gap: 0.75rem;
  }

  .nav-links {
    flex-wrap: wrap;
  }
}
</style>

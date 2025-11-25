<template>
  <div class="app-shell">
    <Header />
    <main class="container">
      <RouterView />
    </main>
  </div>
</template>

<script setup>
import Header from "./components/Header.vue";
</script>

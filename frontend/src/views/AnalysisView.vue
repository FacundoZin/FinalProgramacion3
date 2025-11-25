<template>
  <section>
    <h2>Análisis del estado actual</h2>
    <p>
      Selecciona un cliente para ver sus criptomonedas actuales y su valor
      estimado en ARS.
    </p>

    <div class="filters">
      <label>
        Cliente
        <select v-model.number="clientId">
          <option disabled value="0">Seleccionar cliente</option>
          <option v-for="c in clients" :key="c.id" :value="c.id">
            {{ c.name }} ({{ c.email }})
          </option>
        </select>
      </label>
      <button @click="loadAnalysis" :disabled="!clientId">Analizar</button>
    </div>

    <div v-if="error" class="error">{{ error }}</div>

    <table v-if="items.length">
      <thead>
        <tr>
          <th>Criptomoneda</th>
          <th>Cantidad</th>
          <th>Dinero (ARS)</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="i in items" :key="i.cryptoCode">
          <td>{{ i.cryptoCode }}</td>
          <td>{{ i.amount }}</td>
          <td>{{ i.money }}</td>
        </tr>
      </tbody>
      <tfoot>
        <tr>
          <th colspan="2">Total</th>
          <th>{{ totalMoney }}</th>
        </tr>
      </tfoot>
    </table>

    <p v-else-if="clientId && !loading">
      El cliente no tiene criptomonedas actualmente.
    </p>
  </section>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { API_BASE } from "../apiConfig.js";

const clients = ref([]);
const clientId = ref(0);
const items = ref([]);
const totalMoney = ref(0);
const error = ref("");
const loading = ref(false);

async function loadClients() {
  try {
    const res = await fetch(`${API_BASE}/client`);
    clients.value = await res.json();
  } catch (e) {
    console.error(e);
  }
}

async function loadAnalysis() {
  if (!clientId.value) return;
  error.value = "";
  loading.value = true;
  try {
    const res = await fetch(`${API_BASE}/Analysis/${clientId.value}`);
    if (!res.ok) throw new Error("No se pudo obtener el análisis");
    const data = await res.json();
    items.value = data.items || [];
    totalMoney.value = data.totalMoney || 0;
  } catch (e) {
    error.value = e.message;
  } finally {
    loading.value = false;
  }
}

onMounted(loadClients);
</script>

<style scoped>
.filters {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  margin-bottom: 1rem;
}
</style>

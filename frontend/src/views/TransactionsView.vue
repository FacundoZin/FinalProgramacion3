<template>
  <section>
    <h2>Nueva transacción</h2>
    <p>Registrar una compra o venta de criptomonedas.</p>

    <div v-if="error" class="error">{{ error }}</div>
    <div v-if="success" class="success">{{ success }}</div>

    <form @submit.prevent="createTransaction">
      <label>
        Tipo de transacción
        <select v-model="action">
          <option value="purchase">Compra</option>
          <option value="sale">Venta</option>
        </select>
      </label>

      <label>
        Criptomoneda
        <select v-model="cryptoCode">
          <option value="btc">Bitcoin (BTC)</option>
          <option value="usdc">USDC</option>
          <option value="eth">Ethereum (ETH)</option>
        </select>
      </label>

      <label>
        Cantidad
        <input
          v-model.number="cryptoAmount"
          type="number"
          step="0.00000001"
          min="0"
          required
        />
      </label>

      <label>
        Cliente
        <select v-model.number="clientId">
          <option disabled value="">Seleccionar cliente</option>
          <option v-for="c in clients" :key="c.id" :value="c.id">
            {{ c.name }} ({{ c.email }})
          </option>
        </select>
      </label>

      <label>
        Fecha y hora
        <input v-model="datetime" type="datetime-local" required />
      </label>

      <button type="submit">Guardar transacción</button>
    </form>
  </section>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { API_BASE } from "../apiConfig.js";

const action = ref("purchase");
const cryptoCode = ref("btc");
const cryptoAmount = ref(0);
const clientId = ref("");
const datetime = ref(new Date().toISOString().slice(0, 16));
const clients = ref([]);
const error = ref("");
const success = ref("");

async function loadClients() {
  try {
    const res = await fetch(`${API_BASE}/Client`);
    clients.value = await res.json();
  } catch (e) {
    console.error(e);
  }
}

async function createTransaction() {
  error.value = "";
  success.value = "";

  if (!clientId.value) {
    error.value = "Debes seleccionar un cliente";
    return;
  }
  if (cryptoAmount.value <= 0) {
    error.value = "La cantidad debe ser mayor a 0";
    return;
  }

  try {
    const res = await fetch(`${API_BASE}/Transaction`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        cryptoCode: cryptoCode.value,
        action: action.value,
        clientId: Number(clientId.value),
        cryptoAmount: Number(cryptoAmount.value),
        dateTime: new Date(datetime.value).toISOString(),
      }),
    });

    if (!res.ok) {
      const text = await res.text();
      throw new Error(text || "Error al crear la transacción");
    }

    success.value = "Transacción guardada correctamente";
    cryptoAmount.value = 0;
  } catch (e) {
    error.value = e.message;
  }
}

onMounted(loadClients);
</script>

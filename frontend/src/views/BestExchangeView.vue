
<template>
  <section>
    <h2>¿Dónde comprar o vender?</h2>
    <p>Selecciona una criptomoneda y el tipo de operación para ver en qué exchange conviene.</p>

    <div v-if="error" class="error">{{ error }}</div>

    <form @submit.prevent="findBest">
      <label>
        Criptomoneda
        <select v-model="cryptoCode">
          <option value="btc">Bitcoin (BTC)</option>
          <option value="usdc">USDC</option>
          <option value="eth">Ethereum (ETH)</option>
        </select>
      </label>

      <label>
        Operación
        <select v-model="action">
          <option value="purchase">Comprar</option>
          <option value="sale">Vender</option>
        </select>
      </label>

      <button type="submit">Buscar mejor exchange</button>
    </form>

    <div v-if="result" class="success" style="margin-top:1rem">
      <p>
        Para <strong>{{ action === 'purchase' ? 'comprar' : 'vender' }}</strong>
        <strong>{{ cryptoCode.toUpperCase() }}</strong> conviene usar:
      </p>
      <p>
        <strong>{{ result.exchange }}</strong> con un precio aproximado de
        <strong>{{ result.price }} ARS</strong> por unidad.
      </p>
    </div>
  </section>
</template>

<script setup>
import { ref } from 'vue'
import { API_BASE } from '../apiConfig.js'

const cryptoCode = ref('btc')
const action = ref('purchase')
const result = ref(null)
const error = ref('')

async function findBest () {
  error.value = ''
  result.value = null
  try {
    const params = new URLSearchParams({ cryptoCode: cryptoCode.value, action: action.value })
    const res = await fetch(`${API_BASE}/best-exchange?${params.toString()}`)
    if (!res.ok) {
      const text = await res.text()
      throw new Error(text || 'No se pudo obtener el mejor exchange')
    }
    result.value = await res.json()
  } catch (e) {
    error.value = e.message
  }
}
</script>

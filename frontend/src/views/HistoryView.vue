
<template>
  <section>
    <h2>Historial de movimientos</h2>
    <p>Selecciona un cliente para ver todas sus compras y ventas.</p>

    <div class="filters">
      <label>
        Cliente
        <select v-model.number="selectedClientId">
          <option :value="0">Todos los clientes</option>
          <option v-for="c in clients" :key="c.id" :value="c.id">
            {{ c.name }} ({{ c.email }})
          </option>
        </select>
      </label>
      <button @click="loadTransactions">Buscar</button>
    </div>

    <div v-if="error" class="error">{{ error }}</div>

    <table v-if="transactions.length">
      <thead>
        <tr>
          <th>ID</th>
          <th>Cliente</th>
          <th>Acción</th>
          <th>Cripto</th>
          <th>Cantidad</th>
          <th>Dinero (ARS)</th>
          <th>Fecha y hora</th>
          <th>Acciones</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="t in transactions" :key="t.id">
          <td>{{ t.id }}</td>
          <td>{{ clientName(t.clientId) }}</td>
          <td>{{ t.action }}</td>
          <td>{{ t.cryptoCode }}</td>
          <td>{{ t.cryptoAmount }}</td>
          <td>{{ t.money }}</td>
          <td>{{ new Date(t.dateTime).toLocaleString() }}</td>
          <td class="actions">
            <button @click="viewTransaction(t)">Ver</button>
            <button @click="startEdit(t)">Editar</button>
            <button @click="deleteTransaction(t)">Borrar</button>
          </td>
        </tr>
      </tbody>
    </table>

    <p v-else>No hay transacciones para mostrar.</p>

    <!-- Modal simple de ver -->
    <div v-if="viewing" class="modal">
      <div class="modal-content">
        <h3>Detalle de transacción #{{ viewing.id }}</h3>
        <p><strong>Cliente:</strong> {{ clientName(viewing.clientId) }}</p>
        <p><strong>Acción:</strong> {{ viewing.action }}</p>
        <p><strong>Cripto:</strong> {{ viewing.cryptoCode }}</p>
        <p><strong>Cantidad:</strong> {{ viewing.cryptoAmount }}</p>
        <p><strong>Dinero:</strong> {{ viewing.money }}</p>
        <p><strong>Fecha:</strong> {{ new Date(viewing.dateTime).toLocaleString() }}</p>
        <button @click="viewing = null">Cerrar</button>
      </div>
    </div>

    <!-- Modal simple de edición solo de dinero -->
    <div v-if="editing" class="modal">
      <div class="modal-content">
        <h3>Editar transacción #{{ editing.id }}</h3>
        <label>
          Dinero (ARS)
          <input v-model.number="editingMoney" type="number" step="0.01" />
        </label>
        <div style="margin-top:1rem">
          <button @click="saveEdit">Guardar</button>
          <button @click="cancelEdit">Cancelar</button>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { API_BASE } from '../apiConfig.js'

const clients = ref([])
const transactions = ref([])
const selectedClientId = ref(0)
const error = ref('')

const viewing = ref(null)
const editing = ref(null)
const editingMoney = ref(0)

async function loadClients () {
  try {
    const res = await fetch(`${API_BASE}/clients`)
    clients.value = await res.json()
  } catch (e) {
    console.error(e)
  }
}

async function loadTransactions () {
  error.value = ''
  try {
    let url = `${API_BASE}/transactions`
    if (selectedClientId.value && selectedClientId.value !== 0) {
      url += `?clientId=${selectedClientId.value}`
    }
    const res = await fetch(url)
    if (!res.ok) {
      throw new Error('No se pudieron obtener las transacciones')
    }
    transactions.value = await res.json()
  } catch (e) {
    error.value = e.message
  }
}

function clientName (id) {
  const c = clients.value.find(x => x.id === id)
  return c ? c.name : `#${id}`
}

function viewTransaction (t) {
  viewing.value = { ...t }
}

function startEdit (t) {
  editing.value = { ...t }
  editingMoney.value = t.money
}

function cancelEdit () {
  editing.value = null
}

async function saveEdit () {
  if (!editing.value) return
  try {
    const res = await fetch(`${API_BASE}/transactions/${editing.value.id}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ money: Number(editingMoney.value) })
    })
    if (!res.ok) {
      throw new Error('Error al guardar cambios')
    }
    editing.value = null
    await loadTransactions()
  } catch (e) {
    error.value = e.message
  }
}

async function deleteTransaction (t) {
  if (!confirm(`¿Seguro que deseas borrar la transacción #${t.id}?`)) return
  try {
    const res = await fetch(`${API_BASE}/transactions/${t.id}`, {
      method: 'DELETE'
    })
    if (!res.ok) {
      throw new Error('Error al borrar la transacción')
    }
    await loadTransactions()
  } catch (e) {
    error.value = e.message
  }
}

onMounted(async () => {
  await loadClients()
  await loadTransactions()
})
</script>

<style scoped>
.filters {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  margin-bottom: 1rem;
}
.modal {
  position: fixed;
  inset: 0;
  background: rgba(0,0,0,0.5);
  display: flex;
  align-items: center;
  justify-content: center;
}
.modal-content {
  background: white;
  padding: 1rem 1.5rem;
  border-radius: 4px;
  min-width: 280px;
}
</style>

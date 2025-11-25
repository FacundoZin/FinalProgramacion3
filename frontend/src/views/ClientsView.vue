
<template>
  <section>
    <h2>Dar de alta un nuevo cliente</h2>
    <p>Completa el formulario para registrar un nuevo cliente.</p>

    <div v-if="error" class="error">{{ error }}</div>
    <div v-if="success" class="success">{{ success }}</div>

    <form @submit.prevent="createClient">
      <label>
        Nombre y apellido
        <input v-model="name" type="text" required />
      </label>
      <label>
        Email
        <input v-model="email" type="email" required />
      </label>
      <button type="submit">Guardar cliente</button>
    </form>

    <h3 style="margin-top:2rem">Clientes registrados</h3>
    <table v-if="clients.length">
      <thead>
        <tr>
          <th>ID</th>
          <th>Nombre</th>
          <th>Email</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="c in clients" :key="c.id">
          <td>{{ c.id }}</td>
          <td>{{ c.name }}</td>
          <td>{{ c.email }}</td>
        </tr>
      </tbody>
    </table>
    <p v-else>No hay clientes registrados todavía.</p>
  </section>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { API_BASE } from '../apiConfig.js'

const name = ref('')
const email = ref('')
const error = ref('')
const success = ref('')
const clients = ref([])

async function loadClients () {
  try {
    const res = await fetch(`${API_BASE}/clients`)
    clients.value = await res.json()
  } catch (e) {
    console.error(e)
  }
}

async function createClient () {
  error.value = ''
  success.value = ''
  try {
    const res = await fetch(`${API_BASE}/clients`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ name: name.value, email: email.value })
    })

    if (!res.ok) {
      const text = await res.text()
      throw new Error(text || 'Error al crear el cliente')
    }

    name.value = ''
    email.value = ''
    success.value = 'Cliente creado correctamente'
    await loadClients()
  } catch (e) {
    error.value = e.message
  }
}

onMounted(loadClients)
</script>


# Trabajo Final Programación III - Cartera de Criptomonedas

Este proyecto cumple la consigna del trabajo final:

- Backend en **ASP.NET** (.NET 8) con **SQLite**.
- Frontend en **Vue 3** con Vite.
- Uso de la API de **CriptoYa** para obtener precios en ARS.
- Módulos:
  - Alta de clientes.
  - Alta de compras y ventas de criptomonedas.
  - Historial de movimientos con lectura, edición de monto y borrado.
  - Análisis del estado actual por cliente (mejora).
  - Pantalla "¿Dónde comprar o vender?" (mejora).

## Estructura

- `/backend/CryptoPortfolio.Api` → API REST en ASP.NET.
- `/frontend` → Aplicación Vue 3.

---

## Cómo ejecutar el backend

1. Abrí una terminal en:

   ```bash
   cd backend/CryptoPortfolio.Api
   ```

2. Restaurá dependencias y ejecutá:

   ```bash
   dotnet restore
   dotnet run
   ```

3. La API por defecto queda en `https://localhost:5001` (o la URL que te indique la consola).

4. Endpoints principales (métodos y rutas):

   - `POST /clients` → Crear cliente.
   - `GET /clients` → Listar clientes.
   - `GET /clients/{id}` → Ver cliente.
   - `PATCH /clients/{id}` → Editar cliente.
   - `DELETE /clients/{id}` → Borrar cliente.

   - `POST /transactions` → Crear compra o venta.
   - `GET /transactions?clientId={id}` → Historial (opcional filtrar por cliente).
   - `GET /transactions/{id}` → Ver una transacción.
   - `PATCH /transactions/{id}` → Editar transacción (edición cruda, sin recalcular).
   - `DELETE /transactions/{id}` → Borrar transacción.

   - `GET /analysis/{clientId}` → Análisis de criptos actuales del cliente.
   - `GET /best-exchange?cryptoCode=btc&action=purchase` → Mejora "¿Dónde comprar o vender?".

> La base de datos `crypto.db` se crea automáticamente con SQLite en la carpeta del proyecto.

---

## Cómo ejecutar el frontend

1. Abrí otra terminal en:

   ```bash
   cd frontend
   ```

2. Instalá dependencias:

   ```bash
   npm install
   ```

3. Ejecutá el servidor de desarrollo:

   ```bash
   npm run dev
   ```

4. Abrí el navegador en la URL que indique Vite (normalmente `http://localhost:5173`).

> Si tu backend corre en otra URL distinta de `https://localhost:5001`, cambiá la constante `API_BASE` en los archivos de vistas (`ClientsView.vue`, `TransactionsView.vue`, etc.).

---

## Pantallas implementadas (según consigna)

### 1. Alta de cliente

- Vista: **Clientes**
- Valida:
  - Nombre no vacío.
  - Email no vacío y con `@`.
- Llama a `POST /clients` y muestra el listado de clientes con `GET /clients`.

### 2. Alta de compras y ventas

- Vista: **Nueva transacción**
- Formulario único con selector de **acción**:
  - `purchase` → compra.
  - `sale` → venta.
- Campos:
  - Criptomoneda (BTC, USDC, ETH).
  - Cantidad (decimal > 0).
  - Cliente (select con clientes registrados).
  - Fecha y hora.
- El backend:
  - Valida datos.
  - Para ventas, valida que el cliente tenga saldo suficiente de esa cripto.
  - Consulta precio en ARS a la API de **CriptoYa** (`satoshitango/{crypto}/ars`).
  - Calcula el dinero total (`crypto_amount * precio`).
  - Guarda la transacción en la base.

### 3. Historial de movimientos

- Vista: **Historial**
- Selector de cliente y botón de búsqueda.
- Tabla con:
  - Cliente, acción (compra/venta), cripto, cantidad, dinero, fecha/hora.
- Acciones por fila:
  - **Ver** → abre modal de detalle (usa `GET /transactions/{id}` indirectamente porque ya tenemos los datos).
  - **Editar** → abre modal para editar el campo `money` (usa `PATCH /transactions/{id}`).
  - **Borrar** → confirma y llama a `DELETE /transactions/{id}`.

### 4. Lectura, edición y borrado (backend)

- Implementado con endpoints:
  - `GET /transactions/{id}`
  - `PATCH /transactions/{id}`
  - `DELETE /transactions/{id}`

---

## Mejoras implementadas

### A. Análisis del estado actual

- Vista: **Análisis**
- Seleccionás un cliente y se llama a `GET /analysis/{clientId}`.
- El backend:
  - Suma compras y ventas por criptomoneda.
  - Se queda solo con aquellas donde el saldo > 0.
  - Consulta precio actual en ARS por cripto usando CriptoYa.
  - Devuelve:
    - Criptomoneda, cantidad, dinero en ARS.
    - Total de la cartera.

### B. ¿Dónde comprar o vender?

- Vista: **¿Dónde comprar/vender?**
- Seleccionás:
  - Criptomoneda.
  - Acción: comprar (`purchase`) o vender (`sale`).
- Llama a `GET /best-exchange?cryptoCode=btc&action=purchase`.
- El backend:
  - Consulta varios exchanges (SatoshiTango, Ripio, Belo).
  - Para compras elige el **menor ask**.
  - Para ventas elige el **mayor bid**.
  - Devuelve exchange recomendado y precio.

---

## Notas

- El proyecto usa un único formulario para compras y ventas, tal como permite la consigna (select de acción).
- Las criptomonedas mínimas pedidas son 3 (BTC, USDC, ETH), pero podés agregar más en el frontend sin tocar el backend.
- Para un entorno productivo se podrían agregar:
  - Manejo de usuarios y autenticación.
  - Tests automatizados frontend/backend.
  - Manejo de errores más detallado.
  - Manejo de diferentes monedas además de ARS.


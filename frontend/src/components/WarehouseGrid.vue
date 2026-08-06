<script setup lang="ts">
import { useWarehouseStore } from '@/stores/warehouseStore'

const store = useWarehouseStore()

const headers = [
  { title: 'Klient', key: 'clientName' },
  { title: 'Produkt', key: 'productName' },
  { title: 'Kategorie', key: 'categories' },
  { title: 'Počet kusů', key: 'quantity', sortable: false },
]

const itemsPerPageOptions = [
  { value: 10, title: '10' },
  { value: 20, title: '20' },
  { value: 50, title: '50' },
]

function onUpdateOptions(options: { page: number; itemsPerPage: number }) {
  if (options.itemsPerPage !== store.pageSize) {
    store.setPageSize(options.itemsPerPage)
  } else if (options.page !== store.page) {
    store.setPage(options.page)
  }
}

function onQuantityChange(clientId: number, productId: number, event: Event) {
  const value = Number((event.target as HTMLInputElement).value)
  store.updateQuantity(clientId, productId, value)
}
</script>

<template>
  <v-data-table-server
    :headers="headers"
    :items="store.rows"
    :items-length="store.totalCount"
    :loading="store.loading"
    :items-per-page="store.pageSize"
    :items-per-page-options="itemsPerPageOptions"
    :page="store.page"
    item-value="productId"
    @update:options="onUpdateOptions"
  >
    <template #item.categories="{ item }">
      {{ (item.categories ?? []).join(', ') }}
    </template>
    <template #item.quantity="{ item }">
      <v-text-field
        :model-value="item.quantity"
        type="number"
        min="0"
        density="compact"
        variant="outlined"
        hide-details
        style="max-width: 100px"
        @change="onQuantityChange(item.clientId!, item.productId!, $event)"
      />
    </template>
  </v-data-table-server>

  <v-snackbar
    :model-value="store.errorMessage !== ''"
    color="error"
    timeout="4000"
    @update:model-value="store.errorMessage = ''"
  >
    {{ store.errorMessage }}
  </v-snackbar>
</template>

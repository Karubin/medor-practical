<script setup lang="ts">
import { ref, watch } from 'vue'
import { useWarehouseStore } from '@/stores/warehouseStore'

const store = useWarehouseStore()

const selectedClientIds = ref<number[]>([])
const selectedProductIds = ref<number[]>([])
const selectedCategoryIds = ref<number[]>([])

watch([selectedClientIds, selectedProductIds, selectedCategoryIds], () => {
  store.applyFilters(selectedClientIds.value, selectedProductIds.value, selectedCategoryIds.value)
})
</script>

<template>
  <v-navigation-drawer permanent width="320">
    <v-container>
      <v-autocomplete
        v-model="selectedClientIds"
        :items="store.clients"
        item-title="name"
        item-value="id"
        label="Klienti"
        multiple
        chips
        closable-chips
      />
      <v-autocomplete
        v-model="selectedProductIds"
        :items="store.products"
        item-title="name"
        item-value="id"
        label="Produkty"
        multiple
        chips
        closable-chips
      />
      <v-autocomplete
        v-model="selectedCategoryIds"
        :items="store.categories"
        item-title="name"
        item-value="id"
        label="Kategorie"
        multiple
        chips
        closable-chips
      />
    </v-container>
  </v-navigation-drawer>
</template>

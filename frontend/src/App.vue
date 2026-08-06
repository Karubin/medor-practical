<script setup lang="ts">
import { onMounted } from 'vue'
import FilterPanel from '@/components/FilterPanel.vue'
import WarehouseGrid from '@/components/WarehouseGrid.vue'
import { useWarehouseStore } from '@/stores/warehouseStore'

const store = useWarehouseStore()

onMounted(async () => {
  await store.loadReferenceData()
  await store.loadRows()
})
</script>

<template>
  <v-app>
    <v-app-bar title="Správa skladu" />
    <FilterPanel />
    <v-main>
      <v-container>
        <WarehouseGrid />
      </v-container>
    </v-main>
  </v-app>
</template>

import { defineStore } from 'pinia'
import {
  WarehouseApiClient,
  UpdateQuantityDto,
  type ClientDto,
  type ProductDto,
  type CategoryDto,
  type WarehouseRowDto,
} from '@/api/generated/api-client'

const apiClient = new WarehouseApiClient()

export const useWarehouseStore = defineStore('warehouse', {
  state: () => ({
    clients: [] as ClientDto[],
    products: [] as ProductDto[],
    categories: [] as CategoryDto[],

    selectedClientIds: [] as number[],
    selectedProductIds: [] as number[],
    selectedCategoryIds: [] as number[],

    rows: [] as WarehouseRowDto[],
    totalCount: 0,
    page: 1,
    pageSize: 10,

    loading: false,
    errorMessage: '',
  }),

  actions: {
    async loadReferenceData() {
      this.clients = await apiClient.getClients()
      this.products = await apiClient.getProducts()
      this.categories = await apiClient.getCategories()
    },

    async loadRows() {
      this.loading = true
      try {
        const result = await apiClient.getWarehouseRows(
          this.selectedClientIds,
          this.selectedProductIds,
          this.selectedCategoryIds,
          this.page,
          this.pageSize,
        )
        this.rows = result.items ?? []
        this.totalCount = result.totalCount ?? 0
      } finally {
        this.loading = false
      }
    },

    async applyFilters(clientIds: number[], productIds: number[], categoryIds: number[]) {
      this.selectedClientIds = clientIds
      this.selectedProductIds = productIds
      this.selectedCategoryIds = categoryIds
      this.page = 1
      await this.loadRows()
    },

    async setPage(page: number) {
      this.page = page
      await this.loadRows()
    },

    async setPageSize(pageSize: number) {
      this.pageSize = pageSize
      this.page = 1
      await this.loadRows()
    },

    async updateQuantity(clientId: number, productId: number, quantity: number) {
      const row = this.rows.find((r: WarehouseRowDto) => r.clientId === clientId && r.productId === productId)
      const previousQuantity = row?.quantity

      if (row) row.quantity = quantity

      try {
        await apiClient.updateWarehouseQuantity(clientId, productId, new UpdateQuantityDto({ quantity }))
        this.errorMessage = ''
      } catch (error) {
        if (row && previousQuantity !== undefined) row.quantity = previousQuantity
        this.errorMessage = 'Uložení počtu kusů se nezdařilo, zkuste to znovu.'
      }
    },
  },
})

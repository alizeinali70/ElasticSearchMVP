import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ProductState } from './product.reducer';

export const selectProductState = createFeatureSelector<ProductState>('products');

export const selectAll = createSelector(selectProductState, (s) => s.items);
export const selectResults = createSelector(selectProductState, (s) => s.results);
export const selectLoading = createSelector(selectProductState, (s) => s.loading);
export const selectNormalResults = createSelector(selectProductState, (s) => s.normalResults);

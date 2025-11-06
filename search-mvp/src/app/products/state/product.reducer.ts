import { createReducer, on } from '@ngrx/store';
import { Product } from '../product.model';
import { ProductActions } from './product.actions';

export interface ProductState {
  items: Product[];
  results: Product[];
  loading: boolean;
}

export interface ProductState {
  items: Product[];
  results: Product[];
  normalResults: Product[];
  loading: boolean;
}

export const initialState: ProductState = {
  items: [],
  results: [],
  normalResults: [],
  loading: false,
};

export const productReducer = createReducer(
  initialState,
  on(ProductActions.loadAll, (s) => ({ ...s, loading: true })),
  on(ProductActions.loadAllSuccess, (s, { products }) => ({
    ...s,
    loading: false,
    items: products,
  })),
  on(ProductActions.search, (s) => ({ ...s, loading: true })),
  on(ProductActions.searchSuccess, (s, { results }) => ({ ...s, loading: false, results })),
  on(ProductActions.reindex, (s) => ({ ...s, loading: true })),
  on(ProductActions.reindexSuccess, (s) => ({ ...s, loading: false }))
);

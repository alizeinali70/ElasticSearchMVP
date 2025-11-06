import { createActionGroup, props, emptyProps } from '@ngrx/store';
import { Product } from '../product.model';

export const ProductActions = createActionGroup({
  source: 'Products',
  events: {
    'Load All': emptyProps(),
    'Load All Success': props<{ products: Product[] }>(),
    Search: props<{ q: string }>(),
    'Search Success': props<{ results: Product[] }>(),
    Reindex: emptyProps(),
    'Reindex Success': emptyProps(),
  },
});

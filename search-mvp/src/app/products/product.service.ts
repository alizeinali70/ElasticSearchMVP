import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { Product } from './product.model';

@Injectable({ providedIn: 'root' })
export class ProductService {
  private base = environment.apiBase;

  constructor(private http: HttpClient) {}

  all(): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.base}/api/products`);
    console.log('API Base URL:', this.base);
  }

  search(q: string, size = 10): Observable<Product[]> {
    const params = new HttpParams().set('q', q).set('size', size);
    return this.http.get<Product[]>(`${this.base}/api/products/search`, { params });
  }

  reindex(): Observable<any> {
    return this.http.post(`${this.base}/api/products/reindex`, {});
  }

  seed(): Observable<any> {
    return this.http.post(`${this.base}/api/products/seed`, {});
  }
}

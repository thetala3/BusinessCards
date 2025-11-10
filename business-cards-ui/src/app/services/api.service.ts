import axios, { AxiosInstance } from 'axios';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private axiosClient: AxiosInstance;

  constructor() {
    debugger;
    this.axiosClient = axios.create({
      baseURL: environment.apiBaseUrl,
      headers: { 'Content-Type': 'application/json' },
    });
  }

  async getAllCards() {
    debugger;
    const res = await this.axiosClient.get('/GetAllCards');
    return res.data;
  }

  async deleteCard(id: string) {
    debugger;
    await this.axiosClient.delete(`/DeleteCard/${id}`);
  }

  async addCard(card: any) {
    await this.axiosClient.post('/PostCard', card);
  }
}

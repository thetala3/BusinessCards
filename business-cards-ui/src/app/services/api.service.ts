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
    const res = await this.axiosClient.get('BuisnessCards/GetAllCards');
    return res.data;
  }

  async deleteCard(id: string) {
    debugger;
    await this.axiosClient.delete(`BuisnessCards/DeleteCard/${id}`);
  }

  async addCard(card: any) {
    await this.axiosClient.post('BuisnessCards/PostCard', card);
  }

   async downloadCsv(): Promise<void> {
    const res = await this.axiosClient.get('/Export/export/csv', {
      responseType: 'blob',
    });
    this.triggerDownload(res.data, 'business-cards.csv');
  }

  async downloadXml(): Promise<void> {
    const res = await this.axiosClient.get('/Export/export/xml', {
      responseType: 'blob',
    });
    this.triggerDownload(res.data, 'business-cards.xml');
  }

  private triggerDownload(data: Blob, fileName: string) {
    const url = URL.createObjectURL(data);
    const link = document.createElement('a');
    link.href = url;
    link.download = fileName;
    link.click();
    URL.revokeObjectURL(url);
  }
}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { RecipeGetResponse } from './models/get';
import { RecipeDeleteResponse } from './models/delete';
import { RecipeImportResponse } from './models/import';

@Injectable({providedIn: 'root'})
export class APIService {
    constructor(private httpClient: HttpClient) { }

    private _host = 'https://localhost:7194';

    async getList() {
        try {
            const response = await firstValueFrom(
                this.httpClient.get<RecipeGetResponse>(`${this._host}/v1`)
            );

            if (response) return response;

            return { message: 'Failed request to api.', success: false };
        } catch (err) {
            return { message: 'Exception.', success: false};
        }
    }

    async import(formData: FormData) {
        try {
            const response = await firstValueFrom(
                this.httpClient.post<RecipeImportResponse>(`${this._host}/v1/import`, formData)
            );

            if (response) return response;

            return { message: 'Failed request to api.', success: false };
        } catch (err) {
            return { message: 'Exception.', success: false};
        }
    }

    async delete(id: string) {
        try {
            const response = await firstValueFrom(
                this.httpClient.delete<RecipeDeleteResponse>(`${this._host}/v1/${id}`)
            );

            if (response) return response;

            return { message: 'Failed request to api.', success: false };
        } catch (err) {
            return { message: 'Exception.', success: false};
        }
    }
}

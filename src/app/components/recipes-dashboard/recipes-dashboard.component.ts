import { Component, OnInit } from '@angular/core';
import { APIService } from 'src/app/api/api';
import { RecipeGetResponse } from 'src/app/api/models/get';

@Component({
  selector: 'app-recipes-dashboard',
  templateUrl: './recipes-dashboard.component.html',
  styleUrls: ['./recipes-dashboard.component.css'],
})
export class RecipesDashboardComponent implements OnInit {
  constructor(private api: APIService) {}

  ngOnInit(): void {
    this.ngFetch();
  }

  public response?: RecipeGetResponse = undefined;

  public currentRecipe = -1;
  public checked: number[] = [];

  async ngFetch() {
    this.api.getList().then((r) => {
      this.response = r;
      if (r.recipes?.length ?? -1 > 0) {
        this.currentRecipe = 0;
      }
    });
  }

  async ngDelete(id: string) {
    // confirm first

    if (confirm('Are you sure?')) {
      const response = await this.api.delete(id);

      if (response.success) {
        this.ngFetch();
      }
    }
  }

  async ngImport($event: any) {
    const files = $event.target.files;
    const formData = new FormData();

    for (let i = 0; i < files.length; i++) {
      formData.append('files', files[i] as File, files[i].name);
    }

    const response = await this.api.import(formData);

    // see what it is - add snackbar

    this.ngFetch();
  }
}

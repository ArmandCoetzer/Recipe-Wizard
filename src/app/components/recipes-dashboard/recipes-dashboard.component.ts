import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatSelectionListChange } from '@angular/material/list';
import { MatSnackBar } from '@angular/material/snack-bar';
import { APIService } from 'src/app/api/api';
import { RecipeGetResponse } from 'src/app/api/models/get';

@Component({
  selector: 'app-recipes-dashboard',
  templateUrl: './recipes-dashboard.component.html',
  styleUrls: ['./recipes-dashboard.component.css'],
})
export class RecipesDashboardComponent implements OnInit {
  constructor(private api: APIService, private _snackBar: MatSnackBar) {}

  public response?: RecipeGetResponse = undefined;
  public currentRecipe = -1;
  public steps = new FormControl<number[]>([]);
  public previousSteps: number[] = [];

  ngOnInit(): void {
    this.ngFetch();

    this.steps.valueChanges.subscribe(value => {
      this.getRemovedItem();
    });
  }

  private getRemovedItem() {

    if (this.previousSteps.length > this.steps.value!.length) {
      let missing = this.previousSteps.filter(item => this.steps.value!.indexOf(item) < 0);

      if (missing.length > 0) {
        const missingStep = missing[0];
        const allSteps = this.steps.value!;

        const newSteps = allSteps.filter(item => item < missingStep);

        this.previousSteps = newSteps;
        this.steps.setValue(newSteps);
      }
    } else {
      this.previousSteps =this.steps.value!;
    }
  }



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

    if (response.success) {
      this.openSnackBar('successfully uploaded', 'close');
    } else {
      this.openSnackBar('something went wrong', 'close')
    }



    this.ngFetch();
  }

  openSnackBar(message: string, action: string) {
    this._snackBar.open(message, action, {
      duration: 5000,
    });
  }

  checkDisabled(index: number): boolean {
    if (index == 0) return false;

    if (this.steps.value!.indexOf(index - 1) > -1) return false;

    return true;
  }

  emptySelection() {
    this.previousSteps = [];
    this.steps.setValue(this.previousSteps);
  }
}


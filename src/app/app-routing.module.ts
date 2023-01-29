import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RecipesDashboardComponent } from './components/recipes-dashboard/recipes-dashboard.component';

const routes: Routes = [
  { path: '', redirectTo: '/recipes', pathMatch: 'full' },
  { path: 'recipes',             component: RecipesDashboardComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

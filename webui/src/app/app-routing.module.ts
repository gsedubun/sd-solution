import { NgModule } from '@angular/core';
import {  RouterModule, Routes } from '@angular/router';
import { DistrictComponent } from './district/district.component';


const routes: Routes = [
  { path: 'home', component:DistrictComponent },
  { path: '', redirectTo: '/home', pathMatch: 'full'}
];


@NgModule({
  declarations: [],
  imports: [
     RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }

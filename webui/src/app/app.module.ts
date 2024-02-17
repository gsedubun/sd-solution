import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { InputNumberModule } from 'primeng/inputnumber';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { DialogModule } from 'primeng/dialog';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DropdownModule } from 'primeng/dropdown';
import { RadioButtonModule } from 'primeng/radiobutton';
import { RatingModule } from 'primeng/rating';
import { ToolbarModule } from 'primeng/toolbar';
import { ConfirmationService } from 'primeng/api';
import { CardModule } from 'primeng/card';
import { DataViewModule } from 'primeng/dataview';
import { AppComponent } from './app.component';
import { SalesPersonComponent } from './sales-person/sales-person.component';
import { DistrictComponent } from './district/district.component';
import { AppRoutingModule } from './app-routing.module';
import { RouterModule } from '@angular/router';
import { StoreComponent } from './store/store.component';
import { ToastModule } from 'primeng/toast';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { CommonModule } from '@angular/common';

@NgModule({
    declarations: [
        AppComponent,
        SalesPersonComponent,
        DistrictComponent,
        StoreComponent
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        FormsModule,
        CommonModule,
        ReactiveFormsModule,
        TableModule,
        HttpClientModule,
        HttpClientTestingModule,
        InputTextModule,
        DialogModule,
        ToolbarModule,
        ConfirmDialogModule,
        ToastModule, 
        RatingModule,
        InputNumberModule,
        InputTextareaModule,
        RadioButtonModule,
        DropdownModule,
        ButtonModule,
        CardModule,
        DataViewModule,
        AppRoutingModule,
        RouterModule
    ],
    providers: [ConfirmationService],
    bootstrap: [AppComponent]
})
export class AppModule { }

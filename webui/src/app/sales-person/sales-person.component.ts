import { Component,  Input, OnInit } from '@angular/core';
import { IAddSalesPerson, IAvailableSalesPerson, IDistrict, ISalesPerson } from '../domain/models';
import { SalespersonService } from '../services/salesperson.service';
import { FormBuilder, FormGroup,  Validators } from '@angular/forms';
import { ConfirmationService, MessageService } from 'primeng/api';

@Component({
  selector: 'app-sales-person',
  templateUrl: './sales-person.component.html',
  styleUrls: ['./sales-person.component.css']
})
export class SalesPersonComponent implements OnInit {

  salesPersons: ISalesPerson[] = [];
  availableSalesPersons: IAvailableSalesPerson[] = [];
  private _district: IDistrict;
  @Input('district') 
  set district(value: IDistrict){
    this._district = value;
    if(value==undefined){
      this.salesPersons=[];
      return;
    }
    this.getSalesPersons();
    
  }
  get district(): IDistrict{
    return this._district;
  }
  dialogVisible: boolean = false;
  salesPersonForm :FormGroup;
  formMode: string = '';

  constructor(private salesPersonsvc: SalespersonService,
     private fb: FormBuilder,
     private msgSvc: MessageService,
     private confirmSvc: ConfirmationService) { 
    

  }
  ngOnInit(): void {
    this.salesPersonForm= this.fb.group({
      salesPersonId:[0, Validators.required],
      districtId: 0,
      salesType:['', Validators.required],
    });
  }
  onSubmit(){
    this.salesPersonsvc.addSalesPersonToDistrict(this.salesPersonForm.value)
      .subscribe((data: IAddSalesPerson) => {
        this.msgSvc.add({severity:'info', summary:'Confirmed', detail:'Record added'});
        this.dialogVisible = false;
        this.salesPersonForm.reset();
        this.getSalesPersons();
      });
  }
  confirmDelete(event:any,sales: ISalesPerson){
    console.log(event);
    this.confirmSvc.confirm({
      target: event.target,
      message: `Are you sure you want to delete ${sales.fullName} record?`,
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.salesPersonsvc.deleteSalesPersonDistrict(sales).subscribe((data: ISalesPerson) => {
            this.msgSvc.add({severity:'info', summary:'Confirmed', detail:'Record deleted'});
            this.getSalesPersons();
        });
      }
    });
  }
  showDialog(header:string){
    if(this._district == null){
      return;
    }
    this.dialogVisible = true;
    this.salesPersonForm.patchValue({districtId: this._district.districtId});
    this.salesPersonForm.patchValue({district: this._district.districtName});
    this.formMode = header;
    this.salesPersonsvc.getAvailableSalesPersons(this.district.districtId).subscribe((data: IAvailableSalesPerson[]) => {
      this.availableSalesPersons = data;
    });
  }
  
  getSalesPersons(){
    if(this.district == null){
      return;
    }

    this.salesPersonsvc.getSalespersonsForDistrict(this.district.districtId).subscribe((data: ISalesPerson[]) => {
      this.salesPersons = data;
    });

    
  }
}

import { Component, OnInit } from '@angular/core';
import { IDistrict, ISalesPerson } from '../domain/models';
import { DistrictService } from '../services/district.service';
import { SalespersonService } from '../services/salesperson.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConfirmationService } from 'primeng/api';

@Component({
  selector: 'app-district',
  templateUrl: './district.component.html',
  styleUrls: ['./district.component.css']
})
export class DistrictComponent implements OnInit {
  allDistricts: IDistrict[] = [];
  selectedDistrict: IDistrict | undefined;
  salesPersons: ISalesPerson[] = [];
  dialogVisible: boolean;
  districtForm: FormGroup= {} as FormGroup;
  formMode: string;

  constructor(private districtService: DistrictService,
     private salesPersonsvc: SalespersonService,
     private fb: FormBuilder,
     private confirmSvc: ConfirmationService) { }

  ngOnInit(): void {

    this.districtForm = this.fb.group({
      districtId: 0,
      districtName: ['',Validators.required],
      primarySalesId: [0, Validators.required]
    });
    this.LoadDistricts();

  }
  
  Detail(district: IDistrict){
    this.selectedDistrict = district;
  }
  LoadDistricts(){
    this.districtService.getDistricts().subscribe((data: IDistrict[]) => {
      this.allDistricts = data;
    });
  }
  onSubmit(){
    if(this.formMode === 'Edit'){
      this.districtService.updateDistrict(this.districtForm.value)
      .subscribe(() => {
        this.dialogVisible = false;
        this.districtForm.reset();
        this.LoadDistricts();
        this.selectedDistrict = undefined;
      });
      return;
    }
    this.districtService.addDistrict(this.districtForm.value)
      .subscribe((data: IDistrict) => {
        this.dialogVisible = false;
        this.districtForm.reset();
        this.LoadDistricts();
      });
  }
  getSalesPersons(){
    this.salesPersonsvc.getSalesPersons().subscribe((data: ISalesPerson[]) => {
      this.salesPersons = data;
    });
  }

  deleteDistrict(district: IDistrict){
    this.confirmSvc.confirm({
      message: 'Are you sure you want to delete this district?',
      accept: () => {
        this.districtService.deleteDistrict(district.districtId)
        .subscribe((data) => {
          this.selectedDistrict = undefined;
          this.LoadDistricts();
        });
      }
    });
    
  }
  showEdit(){
    this.formMode = 'Edit';
    console.log(this.selectedDistrict);
    this.dialogVisible = true;
    this.salesPersonsvc.getSalesPersons().subscribe((data: ISalesPerson[]) => {
      this.salesPersons = data;
    });

    this.districtForm.patchValue(this.selectedDistrict);
    
  }
  showDialog(){
    this.formMode = 'Add';
    this.dialogVisible = true;
    this.getSalesPersons();
  }
}

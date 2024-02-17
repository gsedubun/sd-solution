import { Component, OnInit } from '@angular/core';
import { IDistrict, ISalesPerson } from '../domain/models';
import { DistrictService } from '../services/district.service';
import { SalespersonService } from '../services/salesperson.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

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

  constructor(private districtService: DistrictService,
     private salesPersonsvc: SalespersonService,
     private fb: FormBuilder) { }

  ngOnInit(): void {

    this.districtForm = this.fb.group({
      districtId: 0,
      districtName: ['',Validators.required],
      primarySalesId: 0
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
    console.log(this.districtForm.value);
    this.districtService.addDistrict(this.districtForm.value)
      .subscribe((data: IDistrict) => {
        this.allDistricts.push(data);
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

 

  showDialog(){
    console.log('show dialog');
    this.dialogVisible = true;
    this.getSalesPersons();
  }
}

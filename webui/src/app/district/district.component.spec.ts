import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DistrictComponent } from './district.component';
import { DistrictService } from '../services/district.service';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SalespersonService } from '../services/salesperson.service';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { of } from 'rxjs';
import { IDistrict } from '../domain/models';
import { By } from '@angular/platform-browser';

describe('DistrictComponent', () => {
  let component: DistrictComponent;
  let fixture: ComponentFixture<DistrictComponent>;
  let districtServiceMock: jest.Mocked<DistrictService>;
  let salesPersonServiceMock: jest.Mocked<SalespersonService>;

  beforeEach(() => {
    districtServiceMock = {
      getDistricts: jest.fn().mockReturnValue(of([{ districtId: 1, districtName: 'District 1', primarySalesId: 1 , primarySalesFullName: 'Sales 1'}])),
      addDistrict: jest.fn(),
    } as unknown as jest.Mocked<DistrictService>;

    salesPersonServiceMock = {
      getSalesPersons: jest.fn(),
    } as unknown as jest.Mocked<SalespersonService>;


     TestBed.configureTestingModule({
      imports: [ 
        ReactiveFormsModule,
       ],
      declarations: [ DistrictComponent],
      providers: [
        { provide: DistrictService, useValue: districtServiceMock },
        { provide: SalespersonService, useValue: salesPersonServiceMock },
        FormBuilder,
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    })

    fixture = TestBed.createComponent(DistrictComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the districtForm with form controls', () => {
    component.ngOnInit();

    expect(component.districtForm).toBeDefined();
    expect(component.districtForm.controls['districtName']).toBeDefined();
    expect(component.districtForm.controls['primarySalesId']).toBeDefined();
  });

  it('should load districts on ngOnInit', () => {
    const mockDistricts: IDistrict[] = [{ districtId: 1, districtName: 'District 1', primarySalesId: 1 , primarySalesFullName: 'Sales 1'}];
    districtServiceMock.getDistricts.mockReturnValue(of(mockDistricts));

    component.ngOnInit();
    const districtElement = fixture.debugElement.query(
      By.css('h4[ng-reflect-ng-class="District 1"]')
    );

    expect(component.allDistricts).toEqual(mockDistricts);
    expect(districtServiceMock.getDistricts).toHaveBeenCalled();
  });

});

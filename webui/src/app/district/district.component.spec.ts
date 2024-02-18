import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DistrictComponent } from './district.component';
import { DistrictService } from '../services/district.service';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SalespersonService } from '../services/salesperson.service';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { Observable, of } from 'rxjs';
import { IAddDistrict, IDistrict, IEditDistrict } from '../domain/models';
import { By } from '@angular/platform-browser';
import { Confirmation, ConfirmationService } from 'primeng/api';

describe('DistrictComponent', () => {
  let component: DistrictComponent;
  let fixture: ComponentFixture<DistrictComponent>;
  let districtServiceMock: jest.Mocked<DistrictService>;
  let salesPersonServiceMock: jest.Mocked<SalespersonService>;
  let confirmationServiceMock: jest.Mocked<ConfirmationService>;

  beforeEach(() => {
    districtServiceMock = {
      getDistricts: jest.fn().mockReturnValue(of([{ districtId: 1, districtName: 'District 1', primarySalesId: 1 , primarySalesFullName: 'Sales 1'}])),
      addDistrict: jest.fn(),
      updateDistrict: jest.fn(),
      deleteDistrict: jest.fn(),
    } as unknown as jest.Mocked<DistrictService>;

    salesPersonServiceMock = {
      getSalesPersons: jest.fn(),
      getAvailableSalesPersons: jest.fn().mockReturnValue(of([{ salesPersonId: 1, fullName: 'Sales 1' }]))
    } as unknown as jest.Mocked<SalespersonService>;

    confirmationServiceMock = {
      confirm: jest.fn(),      
    } as unknown as jest.Mocked<ConfirmationService>;

     TestBed.configureTestingModule({
      imports: [ 
        ReactiveFormsModule,
       ],
      declarations: [ DistrictComponent],
      providers: [
        { provide: DistrictService, useValue: districtServiceMock },
        { provide: SalespersonService, useValue: salesPersonServiceMock },
        { provide: ConfirmationService, useValue: confirmationServiceMock },
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

  it('should add district on form submission', () => {
    component.ngOnInit();
    const mockDistrict = { districtId: 0, districtName: 'District 1', primarySalesId: 1 };
    districtServiceMock.addDistrict.mockReturnValue(of({ districtId: 1, districtName: 'District 1', primarySalesId: 1 , primarySalesFullName: 'Sales 1'}));
    component.districtForm.setValue(mockDistrict);

    component.onSubmit();
    expect(districtServiceMock.addDistrict).toHaveBeenCalledWith(mockDistrict);
  });

  it('should update district on form submission', () => {
    component.ngOnInit();
    const mockEditDistrict: IEditDistrict = { districtId: 1, districtName: 'District 1', primarySalesId: 1 };

    districtServiceMock.updateDistrict.mockReturnValue(of());
    component.formMode = 'Edit';
    component.districtForm.setValue(mockEditDistrict);

    component.onSubmit();
    expect(districtServiceMock.updateDistrict).toHaveBeenCalledWith(mockEditDistrict);
  });

  it('should show dialog and set districtId=0 on showDialog', () => {
    component.ngOnInit();
    
    salesPersonServiceMock.getSalesPersons.mockReturnValue(of([{ salesPersonId: 1, salesType: 'Type A', fullName: 'Sales 1'}]));

    component.showDialog();
    const districtIdElement = fixture.debugElement.query(
      By.css('input[formControlName="districtId"], input[value="0"]')
    );

    expect(component.dialogVisible).toBe(true);
    expect(component.districtForm.value.districtId).toBe(0);
    expect(districtIdElement).toBeTruthy();
  });

  it('should delete district', () => {
    component.ngOnInit();
    const mockDistrict: IDistrict = { districtId: 1, districtName: 'District 1', primarySalesId: 1 , primarySalesFullName: 'Sales 1'};
    districtServiceMock.deleteDistrict.mockReturnValue(of());

    component.deleteDistrict(mockDistrict);
    
    expect(confirmationServiceMock.confirm).toHaveBeenCalledWith({
      message: 'Are you sure you want to delete this district?',
      accept: expect.anything(),
    });

      // Simulate user accepting the confirmation
      const acceptCallback = confirmationServiceMock.confirm.mock.calls[0][0].accept as Function;
      acceptCallback();

    expect(districtServiceMock.deleteDistrict).toHaveBeenCalledWith(mockDistrict.districtId);
    expect(districtServiceMock.getDistricts).toHaveBeenCalled();

  });

  it('should show dialog and load available salespersons', () => {
    const mockAvailableSalesPersons = [{ salesPersonId: 1, fullName: 'Sales Person 1' }];
    salesPersonServiceMock.getSalesPersons.mockReturnValue(of(mockAvailableSalesPersons));
    component.ngOnInit();

    component.showDialog();
    expect(salesPersonServiceMock.getSalesPersons).toHaveBeenCalled();
  });

  it('should set selectedDistrict on Detail', () => {
    const mockDistrict: IDistrict = { districtId: 1, districtName: 'District 1', primarySalesId: 1 , primarySalesFullName: 'Sales 1'};
    component.Detail(mockDistrict);

    expect(component.selectedDistrict).toEqual(mockDistrict);
  });

  it('should set formMode to Edit on showEdit', () => {
    component.ngOnInit();
    const mockDistrict: IDistrict = { districtId: 1, districtName: 'District 1', primarySalesId: 1 , primarySalesFullName: 'Sales 1'};
    component.selectedDistrict=mockDistrict;

    component.showEdit();
    expect(component.formMode).toBe('Edit');
  });

  it('should set formMode to Add on ngOnInit', () => {
    const mockAvailableSalesPersons = [{ salesPersonId: 1, fullName: 'Sales Person 1' }];
    salesPersonServiceMock.getSalesPersons.mockReturnValue(of(mockAvailableSalesPersons));
    component.ngOnInit();
    component.showDialog();
    
    expect(component.formMode).toBe('Add');
  });

  it('should call getDistricts on ngOnInit', () => {
    component.ngOnInit();
    expect(districtServiceMock.getDistricts).toHaveBeenCalled();
  });
});

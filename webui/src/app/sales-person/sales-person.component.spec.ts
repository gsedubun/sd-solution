import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SalesPersonComponent } from './sales-person.component';
import { SalespersonService } from '../services/salesperson.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormBuilder } from '@angular/forms';
import { IAddSalesPerson, IAvailableSalesPerson, IDistrict, ISalesPerson } from '../domain/models';
import { By } from '@angular/platform-browser';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { of } from 'rxjs';

describe('SalesPersonComponent', () => {
  let component: SalesPersonComponent;
  let fixture: ComponentFixture<SalesPersonComponent>;
  let salesPersonServiceMock: jest.Mocked<SalespersonService>;
  let messageServiceMock: jest.Mocked<MessageService>;
  let confirmationServiceMock: jest.Mocked<ConfirmationService>;

  beforeEach(async () => {

    salesPersonServiceMock = {
      addSalesPersonToDistrict:jest.fn(),
      getSalespersonsForDistrict: jest.fn().mockReturnValue(of([{ salesPersonId: 1, fullName: 'Sales 1', districtId: 1 }])),
      deleteSalesPersonDistrict: jest.fn(),
      getAvailableSalesPersons: jest.fn().mockReturnValue(of([{ salesPersonId: 1, fullName: 'Sales 1', districtId: 1 }])),
    } as unknown as jest.Mocked<SalespersonService>;

    messageServiceMock = {
      add: jest.fn(),
    } as unknown as jest.Mocked<MessageService>;

    confirmationServiceMock = {
      confirm: jest.fn(),
    } as unknown as jest.Mocked<ConfirmationService>;

    await TestBed.configureTestingModule({
      imports: [ HttpClientTestingModule ],
      providers: [ 
        { provide: SalespersonService, useValue: salesPersonServiceMock },
        { provide: MessageService, useValue: messageServiceMock },
        { provide: ConfirmationService, useValue: confirmationServiceMock },
        FormBuilder
      
      ],
      declarations: [ SalesPersonComponent ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SalesPersonComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  
  it('should show dialog and set districtId on showDialog', () => {
    component.ngOnInit();   
    const mockDistrict : IDistrict = { districtId: 1, districtName: 'District 1', primarySalesId: 1 , primarySalesFullName: 'Sales 1'};
    component.district = mockDistrict;

    component.showDialog('Add');
    const districtIdElement = fixture.debugElement.query(
      By.css('input[formControlName="districtId"], input[value="1"]')
    );

    expect(component.dialogVisible).toBe(true);
    expect(component.salesPersonForm.value.districtId).toBe(mockDistrict.districtId);
    expect(districtIdElement).toBeTruthy();

  });

  it('should add salesperson on form submission', () => {
    const mockSalesPerson : IAddSalesPerson = { salesPersonId: 1, salesType: 'Primary', districtId:  1} ;
    salesPersonServiceMock.addSalesPersonToDistrict.mockReturnValue(of(mockSalesPerson));
    component.ngOnInit();

    component.salesPersonForm.setValue({
      salesPersonId: 0,
      districtId: 1,
      salesType: 'Type A',
    });

    component.onSubmit();

    expect(salesPersonServiceMock.addSalesPersonToDistrict).toHaveBeenCalledWith({
      salesPersonId: 0,
      districtId: 1,
      salesType: 'Type A',
    });

    expect(messageServiceMock.add).toHaveBeenCalledWith({
      severity: 'info',
      summary: 'Confirmed',
      detail: 'Record added',
    });

    expect(component.dialogVisible).toBe(false);
    expect(component.salesPersonForm.value).toEqual({ salesPersonId: null, districtId: null, salesType: null }); // reset form
  });


  it('should show dialog and load available salespersons', () => {
    const mockAvailableSalesPersons:IAvailableSalesPerson[] = [
      { salesPersonId: 1, fullName: 'Sales Person 1' },
      { salesPersonId: 2, fullName: 'Sales Person 2' },
    ];
    salesPersonServiceMock.getAvailableSalesPersons.mockReturnValue(of(mockAvailableSalesPersons));
    component.ngOnInit();

    component.district = { districtId: 1, districtName: 'District A', primarySalesId: 1, primarySalesFullName: 'Sales 1'};

    component.showDialog('Add Sales Person');

    expect(component.dialogVisible).toBe(true);
    expect(component.salesPersonForm.value.districtId).toBe(1);
    expect(salesPersonServiceMock.getAvailableSalesPersons).toHaveBeenCalledWith(1);
    expect(component.availableSalesPersons).toEqual(mockAvailableSalesPersons);
  });

});

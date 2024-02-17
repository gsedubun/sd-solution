import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StoreComponent } from './store.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { StoreService } from '../services/store.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IDistrict, IStore } from '../domain/models';
import { of } from 'rxjs';
import { By } from '@angular/platform-browser';

describe('StoreComponent', () => {
  let component: StoreComponent;
  let fixture: ComponentFixture<StoreComponent>;
  let storeServiceMock: jest.Mocked<StoreService>;
  let messageServiceMock: jest.Mocked<MessageService>;
  let confirmationServiceMock: jest.Mocked<ConfirmationService>;

  beforeEach(async () => {

    storeServiceMock = {
      getStoresForDistrict: jest.fn().mockReturnValue(of([{ storeId: 1, storeName: 'Store 1', districtId: 1 }])),
      createStore: jest.fn(),
      deleteStore: jest.fn(),
    } as unknown as jest.Mocked<StoreService>;

    messageServiceMock = {
      add: jest.fn(),
    } as unknown as jest.Mocked<MessageService>;

    confirmationServiceMock = {
      confirm: jest.fn(),
    } as unknown as jest.Mocked<ConfirmationService>;

    await TestBed.configureTestingModule({
      imports: [ ReactiveFormsModule ],
      declarations: [ StoreComponent ],
      providers: [ 
        { provide: StoreService, useValue: storeServiceMock },
        { provide: MessageService, useValue: messageServiceMock },
        { provide: ConfirmationService, useValue: confirmationServiceMock },
        FormBuilder
       ],
       schemas: [CUSTOM_ELEMENTS_SCHEMA]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StoreComponent);
    component = fixture.componentInstance;

  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize storeForm with form controls', () => {
    component.ngOnInit();
    
    expect(component.storeForm).toBeDefined();
    expect(component.storeForm.controls['storeName']).toBeDefined();
    expect(component.storeForm.controls['districtId']).toBeDefined();
  });

  it('should call getStoresForDistrict on district change', () => {
    const mockDistrict: IDistrict = { districtId: 1, districtName: 'District 1', primarySalesId: 1 , primarySalesFullName: 'Sales 1'};
    component.district = mockDistrict;

    expect(storeServiceMock.getStoresForDistrict).toHaveBeenCalledWith(mockDistrict.districtId);
  });

  it('should show dialog and set districtId on showDialog', () => {
    component.ngOnInit();   
    const mockDistrict : IDistrict = { districtId: 1, districtName: 'District 1', primarySalesId: 1 , primarySalesFullName: 'Sales 1'};
    component.district = mockDistrict;

    component.showDialog();
    const districtIdElement = fixture.debugElement.query(
      By.css('[name="districtId"]')
    );

    expect(component.dialogVisible).toBe(true);
    expect(component.storeForm.value.districtId).toBe(mockDistrict.districtId);
    expect(districtIdElement).toBeTruthy();

  });

  it('should call createStore on form submission', () => {
    component.ngOnInit();
    const mockStore = { storeId: 0, storeName: 'Store 1', districtId: 1, districtName: 'District 1'};
    storeServiceMock.createStore.mockReturnValue(of(mockStore));
    component.getStoresForDistrict = jest.fn();

    const storeFValue={
      storeName: 'New Store',
      districtId: 1,
    };

    component.storeForm.setValue(storeFValue);

    component.onSubmit();

    expect(storeServiceMock.createStore).toHaveBeenCalledWith({
      storeName: 'New Store',
      districtId: 1,
    } as IStore);
    
    expect(component.getStoresForDistrict).toHaveBeenCalled(); // Make sure the getStoresForDistrict is called
    expect(component.dialogVisible).toBe(false);
    expect(component.storeForm.value).toEqual({ storeName: null, districtId: null }); // form is reset
    //expect(component.storeForm.reset).toHaveBeenCalledBefore( component.getStoresForDistrict);
    //expect(component.getStoresForDistrict).toHaveBeenCalled(); // Make sure the getStoresForDistrict is called
  });

});

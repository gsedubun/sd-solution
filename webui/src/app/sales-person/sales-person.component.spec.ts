import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SalesPersonComponent } from './sales-person.component';
import { SalespersonService } from '../services/salesperson.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('SalesPersonComponent', () => {
  let component: SalesPersonComponent;
  let fixture: ComponentFixture<SalesPersonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ HttpClientTestingModule ],
      providers: [ SalespersonService, MessageService,ConfirmationService,  ],
      declarations: [ SalesPersonComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SalesPersonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  xit('should create', () => {
    expect(component).toBeTruthy();
  });
});

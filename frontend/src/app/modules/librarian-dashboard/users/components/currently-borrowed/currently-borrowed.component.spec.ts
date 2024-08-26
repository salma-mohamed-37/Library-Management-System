import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CurrentlyBorrowedComponent } from './currently-borrowed.component';

describe('CurrentlyBorrowedComponent', () => {
  let component: CurrentlyBorrowedComponent;
  let fixture: ComponentFixture<CurrentlyBorrowedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CurrentlyBorrowedComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CurrentlyBorrowedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

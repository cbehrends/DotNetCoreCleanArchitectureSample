import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { SidenavListComponent } from './sidenav-list.component';
import {RouterTestingModule} from '@angular/router/testing';
import {Router} from '@angular/router';

describe('SidenavListComponent', () => {
  let component: SidenavListComponent;
  let fixture: ComponentFixture<SidenavListComponent>;
  let router: Router;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule.withRoutes([
          {path: 'orders', component: SidenavListComponent},
          {path: '', component: SidenavListComponent},
          {path: 'services', component: SidenavListComponent},
          {path: 'dummy', component: SidenavListComponent}
        ])
      ],
      declarations: [ SidenavListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    router = TestBed.inject(Router);

    fixture = TestBed.createComponent(SidenavListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create new SideNav component instance', () => {
    expect(component).toBeTruthy();
  });

  it('should contain a Home nav link', () => {
    const compiled = fixture.nativeElement;
    const navLink = compiled.querySelector('#home');
    expect(navLink.textContent).toContain('Home');
  });

  it('home nav link should route to Home page', async () => {
    const compiled = fixture.nativeElement;
    const navLink = compiled.querySelector('#home');
    await router.navigateByUrl('/orders'); // Set to something other than the default route
    expect(router.url).toBe('/orders');
    await navLink.click();
    expect(router.url).toBe('/');
  });

  it('should contain a Orders nav link', () => {
    const compiled = fixture.nativeElement;
    const navLink = compiled.querySelector('#orders');
    expect(navLink.textContent).toContain('Orders');
  });

  it('orders nav link should route to Orders page', async () => {
    const compiled = fixture.nativeElement;
    const navLink = compiled.querySelector('#orders');
    await router.navigateByUrl('/dummy'); // Set to something other than the orders route
    await navLink.click();
    expect(router.url).toBe('/orders');
  });

  it('should contain a Services nav link', () => {
    const compiled = fixture.nativeElement;
    const navLink = compiled.querySelector('#services');
    expect(navLink.textContent).toContain('Services');
  });

  it('service-types nav link should route to Services page', async () => {
    const compiled = fixture.nativeElement;
    const navLink = compiled.querySelector('#services');
    await router.navigateByUrl('/dummy');
    expect(router.url).toBe('/dummy');
    await navLink.click();
    expect(router.url).toBe('/services');
  });

});

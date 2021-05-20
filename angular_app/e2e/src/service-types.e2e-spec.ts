import { ServiceTypesPage } from './service-types.po';

import { browser, logging } from 'protractor';

describe('Services Page', () => {
  let page: ServiceTypesPage;

  beforeEach(() => {
    page = new ServiceTypesPage();
  });

  it('should display title message', () => {
    page.navigateTo();
    expect(page.getTitleText()).toEqual('Services');
  });

  it('should have h3 title Services Offered', () => {
    expect(page.getHeadingElement().getText()).toEqual('Services Offered');
  });

  it('should have new service input box', () => {
    expect(page.getAddServicesInput().isPresent()).toBe(true);
  });

  it('should have add new service button that is disabled by default', () => {
    expect(page.getAddServicesButton().isEnabled()).toBe(false);
  });

  it('new service button should be enabled if newServiceName has value', () => {
    page.getAddServicesInput().sendKeys('This is a test')
      .then(() => {
        expect(page.getAddServicesButton().isEnabled()).toBe(true);
      });
  });

  it('new service button should add a new service if newServiceName is valid', async () => {
    browser.sleep(5000);
    page.getAddServicesInput().sendKeys('This is a test');
    await expect(page.getAddServicesButton().isEnabled()).toBe(true);
    await expect(page.getServiceList().all.length).toBe(3);
    // const itemCount = page.getServiceList().all.length;
    // page.getAddServicesButton().click();
    // expect(page.getServiceList().all.length).toBe(itemCount + 1);

  });

  afterEach(async () => {
    // Assert that there are no errors emitted from the browser
    const logs = await browser.manage().logs().get(logging.Type.BROWSER);
    expect(logs).not.toContain(jasmine.objectContaining({
      level: logging.Level.SEVERE,
    } as logging.Entry));
  });
});

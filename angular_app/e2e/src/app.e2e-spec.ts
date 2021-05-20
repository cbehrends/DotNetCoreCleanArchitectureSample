import { AppPage } from './app.po';
import { browser, logging } from 'protractor';

describe('workspace-project App', () => {
  let page: AppPage;

  beforeEach(() => {
    page = new AppPage();
  });

  it('should render layout component', () => {
    page.navigateTo();
    expect(page.getLayout()).toBeTruthy();
  });

  it('should display title message AngularUI', () => {
    page.navigateTo();
    expect(page.getTitleText()).toEqual('AngularUI');
  });

  afterEach(async () => {
    // Assert that there are no errors emitted from the browser
    const logs = await browser.manage().logs().get(logging.Type.BROWSER);
    expect(logs).not.toContain(jasmine.objectContaining({
      level: logging.Level.SEVERE,
    } as logging.Entry));
  });
});

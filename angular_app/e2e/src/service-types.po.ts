import {browser, element, by, Key, ElementFinder} from 'protractor';

export class ServiceTypesPage {

  navigateTo() {
    return browser.get('/services');
  }

  getTitleText(): Promise<string> {
    return browser.getTitle() as Promise<string>;
  }

  getHeadingElement(): ElementFinder {
    return element(by.tagName('h3'));
  }

  getAddServicesInput(): ElementFinder {
    return element(by.id('newServiceName'));
  }

  getAddServicesButton(): ElementFinder {
    return element(by.id('addNewService'));
  }

  getServiceList(): ElementFinder {
    return element(by.id('serviceList'));
  }
}

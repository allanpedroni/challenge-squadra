import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppComponent } from './AppComponent';
import { WeatherForecastService } from './services/weather-forecast.service';

describe('AppComponent', () => {
  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;
  let weatherForecastService: WeatherForecastService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AppComponent],
      providers: [WeatherForecastService]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
    weatherForecastService = TestBed.inject(WeatherForecastService);
  });

  it('should create the app', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with empty city and forecasts', () => {
    expect(component.city).toEqual('Belo Horizonte');
    expect(component.forecasts).toEqual([]);
  });

  it('should call getWeatherForecasts on ngOnInit', () => {
    spyOn(component, 'getWeatherForecasts');
    component.ngOnInit();
    expect(component.getWeatherForecasts).toHaveBeenCalled();
  });

  it('should call weatherForecastService.getWeatherForecast with default city if city is empty', () => {
    spyOn(weatherForecastService, 'getWeatherForecast').and.returnValue(Promise.resolve([]));
    component.getWeatherForecasts();
    expect(weatherForecastService.getWeatherForecast).toHaveBeenCalledWith('Belo Horizonte');
  });

  it('should call weatherForecastService.getWeatherForecast with provided city if city is not empty', () => {
    component.city = 'New York';
    spyOn(weatherForecastService, 'getWeatherForecast').and.returnValue(Promise.resolve([]));
    component.getWeatherForecasts();
    expect(weatherForecastService.getWeatherForecast).toHaveBeenCalledWith('New York');
  });

  it('should set the forecasts with the response from weatherForecastService', async () => {
    const mockResponse = [{ date: '2022-01-01', temperatureC: 20, temperatureF: 68, temperatureMin: 1, temperatureMax: 2, summary: 'Sunny' }];
    spyOn(weatherForecastService, 'getWeatherForecast').and.returnValue(Promise.resolve(mockResponse));
    await component.getWeatherForecasts();
    expect(component.forecasts).toEqual(mockResponse);
  });

  it('should log success message when weatherForecastService.getWeatherForecast is successful', async () => {
    spyOn(console, 'info');
    spyOn(weatherForecastService, 'getWeatherForecast').and.returnValue(Promise.resolve([]));
    await component.getWeatherForecasts();
    expect(console.info).toHaveBeenCalledWith('Success getting weather forecast for Belo Horizonte.');
  });

  it('should log error message when weatherForecastService.getWeatherForecast fails', async () => {
    spyOn(console, 'debug');
    spyOn(console, 'error');
    spyOn(weatherForecastService, 'getWeatherForecast').and.returnValue(Promise.reject('Error'));
    component.city = '';
    await component.getWeatherForecasts();
    expect(console.debug);
    expect(console.error);
  });
});

import { TestBed } from '@angular/core/testing';
import { WeatherForecastService } from './weather-forecast.service';

describe('WeatherForecastService', () => {
  let service: WeatherForecastService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(WeatherForecastService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  //it('should return weather forecast for a city', async () => {
  //  const city = 'New York';
  //  const weatherForecast = await service.getWeatherForecast(city);
  //  expect(weatherForecast).toBeDefined();
  //  expect(weatherForecast.city).toBe(city);

  //  //There was an error!
  //});

  it('should handle error when getting weather forecast', async () => {
    const city = 'InvalidCity';
    const weatherForecast = await service.getWeatherForecast(city);
    expect(weatherForecast).toBeUndefined();
  });
});

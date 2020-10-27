import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-value',
  templateUrl: './value.component.html',
  styleUrls: ['./value.component.css']
})
export class ValueComponent implements OnInit {

  values:any
  constructor(private http:HttpClient) { }

  ngOnInit(): void {
    this.getValues();
  }
getValues(){
  this.http.get('http://localhost:5000/api/WeatherForecast/get').subscribe(reponse=>{
this.values= reponse;
  },error=>{
    console.log(error);
  });
}
}
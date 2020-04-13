import { Component, OnInit } from '@angular/core';

declare var particlesJS: any;

@Component({
  selector: 'app-x-rand-net',
  templateUrl: './x-rand-net.component.html',
  styleUrls: ['./x-rand-net.component.css']
})
export class XRandNetComponent implements OnInit {

  constructor() { }

  ngOnInit() {
      particlesJS.load('particles-js', 'assets/particles-config.json');
  }

}

import { Opacity } from "../globals/opacity.globals";
import { Visibility } from "../globals/visibility.globals";

const NOTIFICATION_BAR_ID = 'notificationBar';

let bar: HTMLElement | null;

let init = () => bar = document.getElementById(NOTIFICATION_BAR_ID);

let BAR_HIDING_DURATION = 1000;

export function notify(message : string, duration: number = 3800) : void {
  if(!message) return;
  init(); if(!bar) return;
  bar.style.opacity = Opacity.VISIBLE;
  bar.style.visibility = Visibility.VISIBLE;
  bar.innerHTML = message;
  setTimeout(() => hide(), duration);
}

export function hide() : void{
  init(); if(!bar) return;
  bar.style.opacity = Opacity.HIDDEN;
  setTimeout(() => {
    if(!bar) return;
    bar.style.visibility = Visibility.HIDDEN;
  }, BAR_HIDING_DURATION);
}
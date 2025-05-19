import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EventService {
  private novedadesSubject = new Subject<void>();

  // Observable para que los componentes puedan suscribirse
  novedades$ = this.novedadesSubject.asObservable();

  // Método para emitir el evento
  triggerNovedades() {
    this.novedadesSubject.next();
  }
}

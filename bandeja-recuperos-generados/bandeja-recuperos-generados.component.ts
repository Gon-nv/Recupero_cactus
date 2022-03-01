import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { RecuperoService } from '../../../../shared/services/recupero.service';
import { Opcion } from '../../../../shared/models/opcion';
import { BandejaRecupero } from '../../../../shared/models/bandeja-recupero.model';
import { RecuperosGeneradosResultModel } from '../../../../shared/models/recuperos-generados-result.model';
import { Router } from '@angular/router';
import { ExcelService } from '../../../../shared/services/excel.service';
import { distinctUntilChanged } from 'rxjs/operators';
import { NotificacionService } from 'src/app/shared/notificaciones/notificacion.service';

@Component({
  selector: 'app-bandeja-recuperos-generados',
  templateUrl: './bandeja-recuperos-generados.component.html',
  styleUrls: ['./bandeja-recuperos-generados.component.scss']
})
export class BandejaRecuperosGeneradosComponent implements OnInit {
  public form: FormGroup;
  public lsPeriodos: Opcion[] = [];
  public lsProgramas: Opcion[] = [];
  public filtro = new BandejaRecupero();
  public dataSource = new MatTableDataSource<RecuperosGeneradosResultModel>();
  @ViewChild(MatPaginator, { static: true }) public paginador: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;


  constructor(
    private formBuilder: FormBuilder,
    private recuperoService: RecuperoService,
    private router: Router,
    private excelService: ExcelService,
    private notificacionService: NotificacionService
  ) {
  }

  ngOnInit(): void {
    this.recuperoService.obtenerProgramas().subscribe((res) => {
      this.lsProgramas = res;
    });

    this.crearFormulario();
  }

  public crearFormulario(): void {
    this.form = this.formBuilder.group({
      nPrograma: [null],
      nPeriodo: [null],
    });

    this.form.get('nPrograma').valueChanges.pipe(distinctUntilChanged()).subscribe((val) => {
      if (val) {
        this.recuperoService.obtenerPeriodosPorPrograma(val).subscribe((res) => {
          this.lsPeriodos = res;
        });
      }
    });
  }

  public buscar(): void {
    this.armarConsulta();

    this.recuperoService.obtenerRecuperosGenerados(this.filtro).subscribe((res) => {
      this.dataSource.data = res;
      this.customizarPaginador(this.paginador);
      this.dataSource.paginator = this.paginador;
      this.dataSource.paginator.firstPage();
      this.dataSource.sort = this.sort;
    })
  }

  private armarConsulta(): void {
    this.filtro.idPrograma = this.form.get('nPrograma').value;
    this.filtro.idPeriodo = this.form.get('nPeriodo').value;
  }

  public verDetalle(idRecupero: number): void {
    this.router.navigate(['gestion', 'detalle-recupero', idRecupero]);
  }

  public imprimir(): void {
    let titulo = 'Recuperos generados'
    if (this.form.get('nPeriodo').value || this.form.get('nPrograma').value) {
      titulo = this.lsPeriodos.find((x) => x.clave == this.form.get('nPeriodo').value).valor
        + ' - ' +
        this.lsProgramas.find((x) => x.clave == this.form.get('nPrograma').value).valor;
    }
    const json = JSON.parse(JSON.stringify(this.dataSource.data));
    this.excelService.exportAsExcelFile(json, titulo);
  }

  public customizarPaginador(paginator: MatPaginator): void {
    paginator._intl.itemsPerPageLabel = 'Recuperos por página';
    paginator._intl.nextPageLabel = 'Siguiente';
    paginator._intl.previousPageLabel = 'Anterior';
    paginator._intl.firstPageLabel = 'Primer página';
    paginator._intl.lastPageLabel = 'Última página';
  }

  getColumnas(): string [] {
    return ['idRecupero', 'periodo', 'procesado', 'fechaArchivo', 'nombreArchivo', 'usuario', 'acciones'];
  }

  eliminarRecupero(idRecupero: number){
    this.recuperoService.eliminarRecupero(idRecupero).subscribe(
      (result)=>{
        if(result){
          this.notificacionService.openInfoModal(['Se eliminó el recupero'], 'Eliminado exitoso');
        }
      }
    )
  }
}

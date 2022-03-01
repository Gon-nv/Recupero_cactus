using Beneficiarios.Aplicacion.Comandos;
using Beneficiarios.Aplicacion.Consultas.Resultados;
using Beneficiarios.Dominio.IRepositorio;
using Beneficiarios.Dominio.Modelo;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Logger;
using System;
using Infraestructura.Core.Comun.Dato;
using System.Collections.Generic;
using System.Text;

namespace Beneficiarios.Aplicacion.Servicios
{
	public class RecuperoServicio
	{
		private readonly ISesionUsuario _sesionUsuario;
		private readonly IRecuperoRepositorio _recuperoRepositorio;
		private readonly ILogger _log;

		public RecuperoServicio(ISesionUsuario sesionUsuario, IRecuperoRepositorio recuperoRepositorio, ILogger log)
		{
			_recuperoRepositorio = recuperoRepositorio;
			_sesionUsuario = sesionUsuario;
			_log = log;
		}

		public IList<OpcionResultado> ObtenerPeriodos()
		{
			return _recuperoRepositorio.ObtenerPeriodos();
		}

		public IList<OpcionResultado> ObtenerProgramasPorPeriodo(int idPeriodo)
		{
			return _recuperoRepositorio.ObtenerProgramasPorPeriodo(idPeriodo);
		}

		public IList<OpcionResultado> ObtenerPeriodosPorPrograma(int idPrograma)
        {
			return _recuperoRepositorio.ObtenerPeriodosPorPrograma(idPrograma);
        }

		public IList<OpcionResultado> ObtenerProgramas()
        {
			return _recuperoRepositorio.ObtenerProgramas();
        }

		public IList<BandejaRecuperoResultado> ObtenerRecuperos(BandejaRecuperoCommand command)
		{
			return _recuperoRepositorio.ObtenerRecuperos(command, _sesionUsuario.Usuario);
		}

		public ArchivoRecuperoResultado ObtenerArchivoRecupero(BandejaRecuperoCommand command)
		{
			var archivo = new ArchivoRecuperoResultado();
			var lista = _recuperoRepositorio.ObtenerRegistrosRecuperoImprimir(command, _sesionUsuario.Usuario);
			archivo.NombreArchivo = _recuperoRepositorio.ObtenerNombreArchivoRecupero(command, _sesionUsuario.Usuario);
			var texto = "";

			foreach (var registro in lista)
			{
				texto = texto +
					registro.TipoConvenio +
					registro.Sucursal +
					registro.Moneda +
					registro.Sistema +
					registro.NumeroCuenta +
					registro.Importe +
					registro.Fecha +
					registro.NumeroConvenioEmpresa +
					registro.NumeroComprobante +
					registro.Cbu +
					registro.Cuota +
					registro.Usuario + "\n";
			}
			archivo.Datos = Encoding.ASCII.GetBytes(texto);

			return archivo;
		}

		public IList<RecuperosGeneradosResultado> ObtenerRecuperosGenerados(BandejaRecuperosGeneradosCommand command)
		{
			return _recuperoRepositorio.ObtenerRecuperosGenerados(command);
		}

		public RecuperosGeneradosResultado ObtenerRecupero(int idRecupero)
		{
			var res = _recuperoRepositorio.ObtenerCabeceraRecupero(idRecupero);
			res.Detalle = _recuperoRepositorio.ObtenerDetalleRecupero(idRecupero);
			return res;
		}

		public bool PuedeGenerarRecupero(BandejaRecuperoCommand command)
		{
			return _recuperoRepositorio.PuedeGenerarRecupero(command, _sesionUsuario.Usuario);
		}

		public IList<ReglaNegocioResultado> ObtenerReglas(int idEtapa)
		{
			return _recuperoRepositorio.ObtenerReglas(idEtapa);
		}

		public ArchivoRecuperoResultado ObtenerArchivoRecuperoDetalle(int idRecupero)
		{
			var archivo = new ArchivoRecuperoResultado();
			var lista = _recuperoRepositorio.ObtenerRegistrosRecuperoDetalle(idRecupero);
			var texto = "";

			foreach (var registro in lista)
			{
				texto = texto +
					registro.TipoConvenio +
					registro.Sucursal +
					registro.Moneda +
					registro.Sistema +
					registro.NumeroCuenta +
					registro.Importe +
					registro.Fecha +
					registro.NumeroConvenioEmpresa +
					registro.NumeroComprobante +
					registro.Cbu +
					registro.Cuota +
					registro.Usuario + "\n";
			}
			archivo.Datos = Encoding.ASCII.GetBytes(texto);

			return archivo;
		}

		public IList<Cofinanciamiento> ObtenerCofinanciamientosVigentes(int idEtapa)
		{
			return _recuperoRepositorio.ObtenerCofinanciamientosVigentes(idEtapa);
		}

		public bool RegistrarModificacionCofinanciamiento(IList<Cofinanciamiento> lsCofinanciamientos)
		{
			var cuil = _sesionUsuario.Usuario.Cuil;
			foreach(var cof in lsCofinanciamientos)
			{
				_recuperoRepositorio.RegistrarModificacionCofinanciamiento(cof, cuil);
			}
			return true;
		}

		public bool RegistrarBajaCofinanciamiento(IList<Cofinanciamiento> lsCofinanciamientos)
		{
			var cuil = _sesionUsuario.Usuario.Cuil;
			foreach (var cof in lsCofinanciamientos)
			{
				_recuperoRepositorio.RegistrarBajaCofinanciamiento(cof, cuil);
			}
			return true;
		}

		public decimal RegistrarCofinanciamiento(Cofinanciamiento cofinanciamiento)
		{
			var cuil = _sesionUsuario.Usuario.Cuil;
			return _recuperoRepositorio.RegistrarCofinanciamiento(cofinanciamiento, cuil).Valor;
			
		}

		public IList<OpcionResultado> ObtenerEtapasCofinanciamiento()
		{
			return _recuperoRepositorio.ObtenerEtapasCofinanciamiento();
		}

		public DetalleLiquidacionResultado ObtenerDetalleLiquidacion(LiquidacionRecuperoCommand command)
		{
			var res = _recuperoRepositorio.ObtenerCabeceraDetalleLiquidacion(command);
			res.Detalle = _recuperoRepositorio.ObtenerDetalleLiquidacion(command);
			return res;
		}

		public IList<HistoricoFichasResultado> ObtenerHistoricoFicha(int idFicha)
		{
			return _recuperoRepositorio.ObtenerHistoricoFicha(idFicha);
        }


		public IList<OpcionResultado> ObtenerEtapasReglasNegocio()
		{
			return _recuperoRepositorio.ObtenerEtapasReglasNegocio();
		}

		public IList<OpcionResultado> ObtenerTipoPPPReglasNegocio()
		{
			return _recuperoRepositorio.ObtenerTipoPPPReglasNegocio();
		}

		public IList<BandejaReglasNegocioResultado> ObtenerReglasNegocio(BandejaReglaNegocioCommand command)
		{
			return _recuperoRepositorio.ObtenerReglasNegocio(command);
		}

		public IList<OpcionResultado> ObtenerEtapas()
		{
			return _recuperoRepositorio.ObtenerEtapas();
		}
		public IList<OperadorResultado> ObtenerOperadores()
		{
			return _recuperoRepositorio.ObtenerOperadores();
		}
		public IList<OpcionResultado> ObtenerCondiciones()
		{
			return _recuperoRepositorio.ObtenerCondiciones();
		}

		public Id RegistrarReglaNegocio(ReglaNegocioCommand command)
		{
			return _recuperoRepositorio.RegistrarReglaNegocio(command, _sesionUsuario.Usuario.Cuil);
		}

		public Id ModificarReglaNegocio(ReglaNegocioCommand command)
		{
			return _recuperoRepositorio.ModificarReglaNegocio(command, _sesionUsuario.Usuario.Cuil);
		}
		public Id EliminarReglaNegocio(ReglaNegocioCommand command)
		{
			return _recuperoRepositorio.EliminarReglaNegocio(command, _sesionUsuario.Usuario.Cuil);
		}

		public ReglaNegocioCommand ObtenerReglaNegocio(int id)
        {
			return _recuperoRepositorio.ObtenerReglaNegocio(id);
        }

		public string GuardarArchivoRecupero(IList<Archivo> command)
        {
			int contador = 0;
            try
            {
				foreach (Archivo recupero in command)
				{
					_recuperoRepositorio.GuardarArchivoRecupero(recupero);
					contador++;
				}
            }
            catch (Exception e)
            {
				_log.ErrorException("Error al registrar datos del archivo", e, e);
            }

			return $"Se cargaron correctamente {contador} filas del archivo.";
			
        }

		public decimal EliminarRecupero(int idRecupero)
        {
			return _recuperoRepositorio.EliminarRecupero(idRecupero).Valor;
        }

	}
}

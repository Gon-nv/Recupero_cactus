using Beneficiarios.Aplicacion.Comandos;
using Beneficiarios.Aplicacion.Consultas.Resultados;
using Beneficiarios.Aplicacion.Servicios;
using Beneficiarios.Dominio.Modelo;
using System.Collections.Generic;
using System.Web.Http;

namespace Api.Controllers
{
	public class RecuperoController : ApiController
	{
		private readonly RecuperoServicio _recuperoServicio;

		public RecuperoController(RecuperoServicio recuperoServicio)
		{
			_recuperoServicio = recuperoServicio;
		}

		[HttpGet, Route("obtener-periodos")]
		public IList<OpcionResultado> ObtenerPeriodos()
		{
			return _recuperoServicio.ObtenerPeriodos();
		}

		[HttpGet, Route("obtener-programas")]
		public IList<OpcionResultado> ObtenerProgramas()
		{
			return _recuperoServicio.ObtenerProgramas();
		}

		[HttpGet, Route("obtener-periodos-programa/{idPrograma}")]
		public IList<OpcionResultado> ObtenerPeriodosPorPrograma([FromUri] int idPrograma)
		{
			return _recuperoServicio.ObtenerPeriodosPorPrograma(idPrograma);
		}

		[HttpGet, Route("obtener-programas-periodo/{idPeriodo}")]
		public IList<OpcionResultado> ObtenerProgramasPorPeriodo([FromUri] int idPeriodo)
		{
			return _recuperoServicio.ObtenerProgramasPorPeriodo(idPeriodo);
		}

		[HttpPost, Route("obtener-recuperos")]
		public IList<BandejaRecuperoResultado> ObtenerRecuperos([FromBody] BandejaRecuperoCommand command)
		{
			return _recuperoServicio.ObtenerRecuperos(command);
		}

		[HttpPost, Route("obtener-archivo-recupero")]
		public ArchivoRecuperoResultado ObtenerArchivoRecupero([FromBody] BandejaRecuperoCommand command)
		{
			return _recuperoServicio.ObtenerArchivoRecupero(command);
		}

		[HttpPost, Route("obtener-recuperos-generados")]
		public IList<RecuperosGeneradosResultado> ObtenerRecuperosGenerados([FromBody] BandejaRecuperosGeneradosCommand command)
		{
			return _recuperoServicio.ObtenerRecuperosGenerados(command);
		}

		[HttpGet, Route("obtener-detalle-recupero/{id}")]
		public RecuperosGeneradosResultado ObtenerDetalleRecupero([FromUri] int id)
		{
			return _recuperoServicio.ObtenerRecupero(id);
		}

		[HttpPost, Route("puede-generar-recupero")]
		public bool PuedeGenerarRecupero([FromBody] BandejaRecuperoCommand command)
		{
			return _recuperoServicio.PuedeGenerarRecupero(command);
		}

		[HttpGet, Route("obtener-reglas-etapa/{id}")]
		public IList<ReglaNegocioResultado> ObtenerReglasEtapa([FromUri] int id)
		{
			return _recuperoServicio.ObtenerReglas(id);
		}

		[HttpGet, Route("obtener-archivo-recupero-detalle/{id}")]
		public ArchivoRecuperoResultado ObtenerArchivoRecuperoDetalle([FromUri] int id)
		{
			return _recuperoServicio.ObtenerArchivoRecuperoDetalle(id);
		}

		[HttpGet, Route("obtener-cofinanciamientos-vigentes/{id}")]
		public IList<Cofinanciamiento> ObtenerCofinanciamientosVigentes([FromUri] int id)
		{
			return _recuperoServicio.ObtenerCofinanciamientosVigentes(id);
		}

		[HttpPost, Route("registrar-modificacion-cofinanciamientos")]
		public bool RegistrarModificacionCofinanciamiento([FromBody] IList<Cofinanciamiento> cofinanciamientos)
		{
			return _recuperoServicio.RegistrarModificacionCofinanciamiento(cofinanciamientos);
		}

		[HttpPost, Route("registrar-baja-cofinanciamientos")]
		public bool RegistrarBajaCofinanciamiento([FromBody] IList<Cofinanciamiento> cofinanciamientos)
		{
			return _recuperoServicio.RegistrarBajaCofinanciamiento(cofinanciamientos);
		}

		[HttpPost, Route("registrar-cofinanciamiento")]
		public decimal RegistrarCofinanciamiento([FromBody] Cofinanciamiento cofinanciamiento)
		{
			return _recuperoServicio.RegistrarCofinanciamiento(cofinanciamiento);
		}

		[HttpGet, Route("obtener-etapas-cofinanciamientos")]
		public IList<OpcionResultado> ObtenerEtapasCofinanciamiento()
		{
			return _recuperoServicio.ObtenerEtapasCofinanciamiento();
		}

		[HttpPost, Route("obtener-detalle-liquidacion")]
		public DetalleLiquidacionResultado ObtenerDetalleLiquidacion([FromBody] LiquidacionRecuperoCommand command)
		{
			return _recuperoServicio.ObtenerDetalleLiquidacion(command);
		}

		[HttpGet, Route("obtener-historico-fichas/{idFicha}")]
		public IList<HistoricoFichasResultado> ObtenerHistoricoFicha([FromUri] int idFicha)
		{
			return _recuperoServicio.ObtenerHistoricoFicha(idFicha);
        }


		[HttpGet, Route("obtener-etapas-reglas-negocio")]
		public IList<OpcionResultado> ObtenerEtapasReglasNegocio()
		{
			return _recuperoServicio.ObtenerEtapasReglasNegocio();
		}

		[HttpGet, Route("obtener-tipoppp-reglas-negocio")]
		public IList<OpcionResultado> ObtenerTipoPPPReglasNegocio()
		{
			return _recuperoServicio.ObtenerTipoPPPReglasNegocio();
		}

		[HttpPost, Route("obtener-reglas-negocio")]
		public IList<BandejaReglasNegocioResultado> ObtenerReglasNegocio([FromBody] BandejaReglaNegocioCommand command)
		{
			return _recuperoServicio.ObtenerReglasNegocio(command);
		}

		[HttpGet, Route("obtener-etapas")]
		public IList<OpcionResultado> ObtenerEtapas()
		{
			return _recuperoServicio.ObtenerEtapas();
		}

		[HttpGet, Route("obtener-operadores")]
		public IList<OperadorResultado> ObtenerOperadores()
		{
			return _recuperoServicio.ObtenerOperadores();
		}

		[HttpGet, Route("obtener-condiciones")]
		public IList<OpcionResultado> ObtenerCondiciones()
		{
			return _recuperoServicio.ObtenerCondiciones();
		}

		[HttpPost, Route("registrar-regla-negocio")]
		public decimal RegistrarReglaNegocio([FromBody] ReglaNegocioCommand command)
		{
			return _recuperoServicio.RegistrarReglaNegocio(command).Valor;
		}

		[HttpPost, Route("modificar-regla-negocio")]
		public decimal ModificarReglaNegocio([FromBody] ReglaNegocioCommand command)
		{
			return _recuperoServicio.ModificarReglaNegocio(command).Valor;
		}

		[HttpPost, Route("eliminar-regla-negocio")]
		public decimal EliminarReglaNegocio([FromBody] ReglaNegocioCommand command)
		{
			return _recuperoServicio.EliminarReglaNegocio(command).Valor;
		}

		[HttpGet, Route("obtener-regla-negocio/{id}")]
		public ReglaNegocioCommand ObtenerReglaNegocio([FromUri] int id)
		{
			return _recuperoServicio.ObtenerReglaNegocio(id);
		}
		[HttpPost, Route("guardar-archivo")]
		public string GuardarArchivoRecupero( IList<Archivo> archivo)
        {
			return _recuperoServicio.GuardarArchivoRecupero(archivo);
        }

		[HttpGet, Route("eliminar-recupero")]
		public decimal EliminarRecupero(int idRecupero)
		{
			return _recuperoServicio.EliminarRecupero(idRecupero);
		}
	}
}

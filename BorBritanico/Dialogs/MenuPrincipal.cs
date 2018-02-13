using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BorBritanico.Dialogs
{
    [Serializable]
    public class MenuPrincipal : IDialog
    {
        const string SiOption = "Sí, Gracias";
        const string NoOption = "No, Necesito Ayuda";
        //private static readonly object context;

        

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceived);
        }

        private async Task MessageReceived(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Hola soy Amy 👩‍💻, la asistente virtual demo , permíteme ayudarte en los siguientes temas:");

            //Llamado del Carousel
            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = GetCards();

            await context.PostAsync(reply);
            context.Wait(OptionResult);
           

        }

        private async Task OptionResult(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result as Activity;

            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = GetBusDoc();

            if (message.Text.Equals("BusDoc"))
            {
                await context.PostAsync("¡Muy bien! Para realizar la búsqueda de documentos deberá de dirigirse a la parte superior derecho del Intranet y seleccionar el icono y colocar el nombre del documento en la barra de búsqueda.");
                await context.PostAsync(reply);
               // await context.PostAsync("Fue útil la solución brindada");
                PromptDialog.Choice(
                            context,
                            this.RegrMenPrin,
                            new[] { SiOption, NoOption },
                                    "Fue útil la solución brindada",
                                    "Selecciona una Opción",
                                    promptStyle: PromptStyle.Auto);
                //context.Wait(RegrMenPrin);

            }
            else if (message.Text.Equals("MesAyu"))
            {
                await context.PostAsync("¡Correcto!. Te presentamos una guía con las opciones que te ofrece mesa de ayuda.");

            }
            else
            {
                await context.PostAsync("Opción Invalida");
            }
        }

        private async Task RegrMenPrin(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var selection = await result;

                switch (selection)
                {
                    case SiOption:
                        await context.PostAsync("Que tengas un buen día");
                        context.Wait(MessageReceived);
                        
                        break;

                    case NoOption:
                        context.Wait(MessageReceived);
                        break;
                }
            }
            catch (Exception e)
            {
                await context.PostAsync(e.Message);
            }
        }

        //Lista de Mantenimiento
        private static IList<Attachment> GetBusDoc()
        {
            return new List<Attachment>()
            {
                BusDocOption()
                
            };
        }

        


        private static Attachment BusDocOption()
        {
            var heroCard = new HeroCard
            {
                Title = "",
                Subtitle = "",
                Text = "",
                Images = new List<CardImage> { new CardImage("https://www.exaltra.com/images/business-it-support.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,"Ir a Intranet" , value: "https://campus.britanico.edu.pe/psp/CS90PRD/?cmd=login&languageCd=ESP&") }
            };
            return heroCard.ToAttachment();
        }
        
        
        //Para llenar la lista Principal
        private static IList<Attachment> GetCards()
        {
            return new List<Attachment>()
            {
                GetHeroCard(),
                GetThumbnailCard(),
                GetSoluRap()
            };
        }

        //Lista de Mantenimiento


        //Opciones para el Carousel
        private static Attachment GetHeroCard()
        {
            var heroCard = new HeroCard
            {
                Title = "Intranet",
                Subtitle = "",
                Text = "",
                Images = new List<CardImage> { new CardImage("https://mvpcluster.com/wp-content/uploads/2016/04/Intranet.png") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.PostBack, "Búsqueda de Documento",value: "BusDoc"), new CardAction(ActionTypes.PostBack, "Mesa de Ayuda", value: "MesAyu")}
            };
            return heroCard.ToAttachment();
        }

                    
        private static Attachment GetThumbnailCard()
        {
            var heroCard = new HeroCard
            {
                Title = "Información de Un Colaborador",
                Subtitle = "",
                Text = "",

                Images = new List<CardImage> { new CardImage("http://www.hinchablesleon.es/wp-content/uploads/2013/03/Contacto.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.MessageBack, "Correo", value: "Prueba 1"), new CardAction(ActionTypes.OpenUrl, "Código", value: "https://docs.microsoft.com/bot-framework"), new CardAction(ActionTypes.OpenUrl, "Anexo y Área", value: "https://docs.microsoft.com/bot-framework") }
            };
            return heroCard.ToAttachment();
        }

        private static Attachment GetSoluRap()
        {
            var heroCard = new HeroCard
            {
                Title = "Soluciones Rápidas de TI",
                Subtitle = "",
                Text = "",
                Images = new List<CardImage> { new CardImage("https://www.exaltra.com/images/business-it-support.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "Ver Opciones", value: "https://docs.microsoft.com/bot-framework") }
            };
            return heroCard.ToAttachment();
        }

    }
}
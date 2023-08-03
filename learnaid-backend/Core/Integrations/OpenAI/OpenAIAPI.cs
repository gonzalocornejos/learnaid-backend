namespace learnaid_backend.Core.Integrations.OpenAI
{
    using Interfaces;
    using learnaid_backend.Configuration;
    using learnaid_backend.Core.DataTransferObjects.Ejercicio;
    using Microsoft.Extensions.Options;
    using OpenAI;
    using System.Threading.Tasks;

    public class OpenAIAPI : IOpenAIAPI
    {
        private readonly OpenAIConfiguration _openAIConfiguration;

        public OpenAIAPI(IOptionsMonitor<OpenAIConfiguration> optionsMonitor)
        {
            _openAIConfiguration = optionsMonitor.CurrentValue;
        }


        public async Task<string> AdaptarEjercicio(EjercicioPorAdaptarDTO ejercicio, string profesion)
        {
            // api instance
            var api = new OpenAI_API.OpenAIAPI(_openAIConfiguration.Key);
            var chat = api.Chat.CreateConversation();

            /// give instruction as System
            var system = "Sos un " + profesion + " de niños de " + ejercicio.EdadAlumnos + " años. Tenes alumnos con dislexia. Tenes que adaptar ejercicios para ellos. La adaptacion tiene que simplificar el texto del ejercicio, si se puede acortarlo mejor, pero nunca alargarlo ni cambiar su contenido, separar los ejercicios en items individuales y poner opciones multiples para las respuestas. Las opciones multiples tienen que ser todas posibles respuestas validas y la opcion correcta no tiene que estar siempre en el mismo lugar.Que haya solo un espacio para completar por ejercicio. Que todas las palabras clave del ejercicio esten en mayusculas para resaltar su importancia.";
            chat.AppendSystemMessage(system);

            // give a few examples as user and assistant
            chat.AppendUserInput("El ejercicio esta en ingles. La estructura de tu respuesta tiene que ser: consigna-ejercicio-opciones. El ejercicio es el siguiente: CONSIGNA:FILL IN THE BLANKS WITH THE VERBS IN THE PAST SIMPLE: EJERCICIO:What __________ you _____________(do) last Saturday? I ______________________(visit) my grandparents.I ____________________(not go) alone.My brother _________________(go) with me.");
            chat.AppendExampleChatbotOutput("CONSIGNA: Completa los espacios en blanco con los verbos en pasado simple. \n EJERCICIO: \n What __________ you _____________(do) last Saturday? \n OPCIONES: \n \t \t \t a) did, do \n \t \t \t b) did, did \n \t \t \t c) do, did \n \t \t \t d) did, did not \n EJERCICIO: \n I ______________________(visit) my grandparents. \n OPCIONES: \n \t \t \t a) visited \n \t \t \t b) visit \n \t \t \t c) visits \n \t \t \t d) visiting \n EJERCICIO: \n I ____________________(not go) alone. \n OPCIONES: \n \t \t \t a) did not go \n \t \t \t b) don't go \n \t \t \t c) didn't go \n \t \t \t d) not go \n EJERCICIO: \n My brother _________________(go) with me. \n OPCIONES: \n \t \t \t a) went \n \t \t \t b) go \n \t \t \t c) goes \n \t \t \t d) going");
            chat.AppendUserInput("El ejercicio esta en español.La estructura de tu respuesta tiene que ser: consigna - ejercicio - opciones.CONSIGNA: Resuelva las siguientes ecuaciones: EJERCICIO: 2 - x = x - 8");
            chat.AppendExampleChatbotOutput("CONSIGNA: Resuelve la siguiente ecuación: \n EJERCICIO: \n 2 - x = x - 8 \n \t \t \t A) x = -5 \n \t \t \t B) x = 5 \n \t \t \t C) x = -3 \n \t \t \t D) x = 3");

            // now let's ask it a question'
            var estructura = "";
            if(ejercicio.Texto == "")
            {
                estructura = "consigna-ejercicio-opciones";
            } else
            {
                estructura = "consigna-texto-ejercicio-opciones";
            }

            var pregunta = "El ejercicio esta en" + ejercicio.Idioma + ". Tu respuesta tiene que estar escrita en ese idioma. La estructura de tu respuesta tiene que ser:" + estructura + ". El ejercicio es el siguiente: CONSIGNA:" + ejercicio.Consigna + " EJERCICIO:" + ejercicio.Ejercicio;
            chat.AppendUserInput(pregunta);
            // and get the response
            string response = await chat.GetResponseFromChatbotAsync();
            Console.WriteLine(response);
            return (response);
        }

        public async Task<EjercicioAdaptadoDTO> AdaptarEjercicioPorPartes(EjercicioPorAdaptarDTO ejercicio, string profesion)
        {
            // api instance
            var api = new OpenAI_API.OpenAIAPI(_openAIConfiguration.Key);
            var chat = api.Chat.CreateConversation();

            /// give instruction as System
            var system = "Sos un " + profesion + " de niños de " + ejercicio.EdadAlumnos + " años. Tenes alumnos con dislexia. Tenes que adaptar ejercicios para ellos. La adaptacion tiene que simplificar el texto del ejercicio, si se puede acortarlo mejor, pero nunca alargarlo ni cambiar su contenido, separar los ejercicios en items individuales y poner opciones multiples para las respuestas. Las opciones multiples tienen que ser todas posibles respuestas validas y la opcion correcta no tiene que estar siempre en el mismo lugar.Que haya solo un espacio para completar por ejercicio. Que todas las palabras clave del ejercicio esten en mayusculas para resaltar su importancia. Los ejercicios y preguntas tienen que ser los mismos, no se pueden cambiar. No agreges palabras extra como opciones, ejercicio, preguntas,etc. Todo el texto se tiene que mantener los mas simple posible. Agregar tabulaciones a las opciones. Acortar la cantidad de ejrcicios y/o preguntas a entre la mitad y tres tercios del original. Si el ejercicio ya tiene opciones no modificarlas ni agregar nuevas.";
            chat.AppendSystemMessage(system);

            // give a few examples as user and assistant
            chat.AppendUserInput("Adapta la siguiente consigna: FILL IN THE BLANKS WITH THE VERBS IN THE PAST SIMPLE: ");
            chat.AppendExampleChatbotOutput("FILL in the blanks with the verbs in the PAST SIMPLE:");
            chat.AppendUserInput("Adapta el siguiente ejercicio en base a la consigna: FILL IN THE BLANKS WITH THE VERBS IN THE PAST SIMPLE:. El ejercicio es el siguiente: What __________ you _____________(do) last Saturday? I ______________________(visit) my grandparents.I ____________________(not go) alone.My brother _________________(go) with me.");
            chat.AppendExampleChatbotOutput("1)What __________ you _____________(do) last Saturday? \n \n \t \t \t a) did, do \n \t \t \t b) did, did \n \t \t \t c) do, did \n \t \t \t d) did, did not \n \n 2)I ______________________(visit) my grandparents. \n \n \t \t \t a) visited \n \t \t \t b) visit \n \t \t \t c) visits \n \t \t \t d) visiting \n \n 3) I ____________________(not go) alone. \n \n \t \t \t a) did not go \n \t \t \t b) don't go \n \t \t \t c) didn't go \n \t \t \t d) not go \n \n 4) My brother _________________(go) with me. \n \n \t \t \t a) went \n \t \t \t b) go \n \t \t \t c) goes \n \t \t \t d) going");
            chat.AppendUserInput("Adapta la siguiente consigna: Resuelva las siguientes ecuaciones:");
            chat.AppendExampleChatbotOutput("RESUELVA la siguiente ecuación: ");
            chat.AppendUserInput("Adapta el siguiente ejercicio en base a la consigna: Resuelva las siguientes ecuaciones:. El ejercicio es el siguiente: 2 - x = x - 8");
            chat.AppendExampleChatbotOutput("1) 2 - x = x - 8 \n \t \t \t A) x = -5 \n \t \t \t B) x = 5 \n \t \t \t C) x = -3 \n \t \t \t D) x = 3");

            // now let's ask it a question'

            var respuesta = new EjercicioAdaptadoDTO();
            respuesta.Titulo = ejercicio.Titulo;

            var pregunta = "Adapta la siguiente consigna: "+ ejercicio.Consigna;
            chat.AppendUserInput(pregunta);
            // and get the response
            string response = await chat.GetResponseFromChatbotAsync();
            Console.WriteLine(response);
            respuesta.Consigna = response;

            if (ejercicio.Texto != "")
            {
                pregunta = "Adapta el siguiente texto en base a la consigna: " + ejercicio.Consigna+". No incluyas la consigna en la respuesta. El texto es el siguiente: "+ ejercicio.Texto;
                chat.AppendUserInput(pregunta);
                // and get the response
                response = await chat.GetResponseFromChatbotAsync();
                Console.WriteLine(response);
                respuesta.Ejercicio = response;
                pregunta = "Adapta el siguiente ejercicio en base a la consigna: " + ejercicio.Consigna + ejercicio.Texto+ ". El ejercicio es el siguiente: " + ejercicio.Ejercicio;
                chat.AppendUserInput(pregunta);
                // and get the response
                response = await chat.GetResponseFromChatbotAsync();
                Console.WriteLine(response);
                respuesta.Ejercicio = respuesta.Ejercicio + "\n"+response;
            } else {
                pregunta = "Adapta el siguiente ejercicio en base a la consigna: " + ejercicio.Consigna +". El ejercicio es el siguiente: " + ejercicio.Ejercicio;
                chat.AppendUserInput(pregunta);
                // and get the response
                response = await chat.GetResponseFromChatbotAsync();
                Console.WriteLine(response);
                respuesta.Ejercicio = response;
            }
            return (respuesta);
        }
    }
}

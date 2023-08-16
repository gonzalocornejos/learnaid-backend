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

        /*
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
        }*/

        public async Task<EjercicioAdaptadoDTO> AdaptarEjercicioPorPartes(EjercicioPorAdaptarDTO ejercicio, string profesion, int edad, string idioma)
        {

            var respuesta = new EjercicioAdaptadoDTO();
            var adaptacionParcial = ejercicio;
            foreach(var adaptacion in ejercicio.Adaptaciones)
            {
                if(adaptacion == "Simplificar Texto")
                {
                    respuesta = await SimplificarTexto(adaptacionParcial, profesion, edad, idioma);
                    adaptacionParcial.Consigna = respuesta.Consigna;
                    adaptacionParcial.Ejercicio = respuesta.Ejercicio;
                } else if (adaptacion == "Agregar opciones multiples"){
                    respuesta = await AgregarMultipleChoice(adaptacionParcial,profesion,edad, idioma);
                    adaptacionParcial.Consigna = respuesta.Consigna;
                    adaptacionParcial.Ejercicio = respuesta.Ejercicio;
                } else if (adaptacion == "Acortar ejercicio")
                {
                    respuesta = await AcortarEjercicio(adaptacionParcial, profesion, edad, idioma);
                    adaptacionParcial.Consigna = respuesta.Consigna;
                    adaptacionParcial.Ejercicio = respuesta.Ejercicio;
                }
            }
            respuesta = await AdaptacionGeneral(adaptacionParcial, profesion, edad, idioma);
            return respuesta;
        }

        public async Task<EjercicioAdaptadoDTO> AdaptacionGeneral(EjercicioPorAdaptarDTO ejercicio, string profesion, int edad, string idioma)
        {
            // api instance
            var api = new OpenAI_API.OpenAIAPI(_openAIConfiguration.Key);
            var chat = api.Chat.CreateConversation();

            /// give instruction as System
            var system = "Sos un " + profesion + " de niños de " + edad + " años. Tenes alumnos con dislexia. Tenes que adaptar ejercicios para ellos. La adaptacion tiene que separar los ejercicios en items individuales .Que haya solo un espacio para completar por ejercicio. Que todas las palabras clave del ejercicio esten en mayusculas para resaltar su importancia. Los ejercicios y preguntas tienen que ser los mismos, no se pueden cambiar. No agreges palabras extra como opciones, ejercicio, preguntas,etc. Agregar tabulaciones a las opciones. Si el ejercicio ya tiene opciones no modificarlas ni agregar nuevas.";
            chat.AppendSystemMessage(system);

            // give a few examples as user and assistant
            chat.AppendUserInput("Adapta la siguiente consigna: FILL IN THE BLANKS WITH THE VERBS IN THE PAST SIMPLE: ");
            chat.AppendExampleChatbotOutput("FILL in the blanks with the verbs in the PAST SIMPLE:");
            chat.AppendUserInput("Adapta el siguiente ejercicio en base a la consigna: FILL IN THE BLANKS WITH THE VERBS IN THE PAST SIMPLE:. El ejercicio es el siguiente: What __________ you _____________(do) last Saturday? I ______________________(visit) my grandparents.I ____________________(not go) alone.My brother _________________(go) with me.");
            chat.AppendExampleChatbotOutput("1)What __________ you _____________(do) LAST Saturday? \n 2)I ______________________(visit) my grandparents. \n 3) I ____________________(not go) alone. \n 4) My brother _________________(go) with me. ");
            chat.AppendUserInput("Adapta la siguiente consigna: Resuelva las siguientes ecuaciones:");
            chat.AppendExampleChatbotOutput("RESUELVA la siguiente ecuación: ");
            chat.AppendUserInput("Adapta el siguiente ejercicio en base a la consigna: Resuelva las siguientes ecuaciones:. El ejercicio es el siguiente: 2 - x = x - 8 \n \t \t \t A) x = -5 \n \t \t \t B) x = 5 \n \t \t \t C) x = -3 \n \t \t \t D) x = 3");
            chat.AppendExampleChatbotOutput("1) 2 - x = x - 8 \n \t \t \t A) x = -5 \n \t \t \t B) x = 5 \n \t \t \t C) x = -3 \n \t \t \t D) x = 3");

            // now let's ask it a question'

            var respuesta = await GenerarRespuesta(ejercicio, chat, idioma);
            return (respuesta);

        }

        public async Task<EjercicioAdaptadoDTO> AcortarEjercicio(EjercicioPorAdaptarDTO ejercicio, string profesion, int edad, string idioma)
        {
            // api instance
            var api = new OpenAI_API.OpenAIAPI(_openAIConfiguration.Key);
            var chat = api.Chat.CreateConversation();

            /// give instruction as System
            var system = "Sos un " + profesion + " de niños de " + edad + " años. Tenes alumnos con dislexia. Tenes que adaptar ejercicios para ellos. La adaptacion tiene que consistir en reducir la cantidad de elementos del ejercicio. No tenes que modificar ningun contenido de nada. Y tampoco tenes que agregar palabaras como Consigna, Ejercicio, Texto, etc. Solo responde con la adaptacion";
            chat.AppendSystemMessage(system);

            // give a few examples as user and assistant
            chat.AppendUserInput("Adapta la siguiente consigna: FILL IN THE BLANKS WITH THE VERBS IN THE PAST SIMPLE: ");
            chat.AppendExampleChatbotOutput("FILL IN THE BLANKS WITH THE VERBS IN THE PAST SIMPLE: ");
            chat.AppendUserInput("Adapta el siguiente ejercicio en base a la consigna: FILL IN THE BLANKS WITH THE VERBS IN THE PAST SIMPLE:. El ejercicio es el siguiente: What __________ you _____________(do) last Saturday? I ______________________(visit) my grandparents.I ____________________(not go) alone.My brother _________________(go) with me.");
            chat.AppendExampleChatbotOutput("1)What __________ you _____________(do) last Saturday? \n \n \t \t \t a) did, do \n \t \t \t b) did, did \n \t \t \t c) do, did \n \t \t \t d) did, did not \n \n 2)I ______________________(visit) my grandparents. \n \n \t \t \t a) visited \n \t \t \t b) visit \n \t \t \t c) visits \n \t \t \t d) visiting \n \n 3) I ____________________(not go) alone. \n \n \t \t \t a) did not go \n \t \t \t b) don't go \n \t \t \t c) didn't go \n \t \t \t d) not go \n \n 4) My brother _________________(go) with me. \n \n \t \t \t a) went \n \t \t \t b) go \n \t \t \t c) goes \n \t \t \t d) going");
            chat.AppendUserInput("Adapta la siguiente consigna: Resuelva las siguientes ecuaciones:");
            chat.AppendExampleChatbotOutput("RESUELVA la siguiente ecuación: ");
            chat.AppendUserInput("Adapta el siguiente ejercicio en base a la consigna: Resuelva las siguientes ecuaciones:. El ejercicio es el siguiente: 2 - x = x - 8");
            chat.AppendExampleChatbotOutput("1) 2 - x = x - 8 \n \t \t \t A) x = -5 \n \t \t \t B) x = 5 \n \t \t \t C) x = -3 \n \t \t \t D) x = 3");

            // now let's ask it a question'

            var respuesta = await GenerarRespuesta(ejercicio, chat, idioma);
            return (respuesta);
        }

        public async Task<EjercicioAdaptadoDTO> AgregarMultipleChoice(EjercicioPorAdaptarDTO ejercicio, string profesion, int edad, string idioma)
        {
            // api instance
            var api = new OpenAI_API.OpenAIAPI(_openAIConfiguration.Key);
            var chat = api.Chat.CreateConversation();

            /// give instruction as System
            var system = "Sos un " + profesion + " de niños de " + edad + " años. Tenes alumnos con dislexia. Tenes que adaptar ejercicios para ellos. La adaptacion tiene que consistir en agregar opciones multiples para las respuestas. Las respuestas tienen que ser 2 o 3 si los alumnos son menores a 9 años y 4 si son mayores. Todas las opciones tienen que ser posibles mas alla de que una sea la correcta. La correcta no tiene que estar siempre en la misma ubicacion. Respetar el idioma del ejercicio. No tenes que modificar ningun contenido de nada. Y tampoco tenes que agregar palabaras como Consigna, Ejercicio, Texto, etc. Solo responde con la adaptacion";
            chat.AppendSystemMessage(system);

            // give a few examples as user and assistant
            chat.AppendUserInput("Adapta la siguiente consigna: FILL IN THE BLANKS WITH THE VERBS IN THE PAST SIMPLE: ");
            chat.AppendExampleChatbotOutput("FILL IN THE BLANKS WITH THE VERBS IN THE PAST SIMPLE: ");
            chat.AppendUserInput("Adapta el siguiente ejercicio en base a la consigna: FILL IN THE BLANKS WITH THE VERBS IN THE PAST SIMPLE:. El ejercicio es el siguiente: What __________ you _____________(do) last Saturday? I ______________________(visit) my grandparents.I ____________________(not go) alone.My brother _________________(go) with me.");
            chat.AppendExampleChatbotOutput("1)What __________ you _____________(do) last Saturday? \n 2)I ______________________(visit) my grandparents. \n ");
            chat.AppendUserInput("Adapta la siguiente consigna: Resuelva las siguientes ecuaciones:");
            chat.AppendExampleChatbotOutput("RESUELVA la siguiente ecuación: ");
            chat.AppendUserInput("Adapta el siguiente ejercicio en base a la consigna: Resuelva las siguientes ecuaciones:. El ejercicio es el siguiente: 2 - x = x - 8  3x+1=7  2x+3=9 x+4=15 9-7+x=4");
            chat.AppendExampleChatbotOutput("1) 2 - x = x - 8 \n 2) 3x + 1 = 7 \n 3) 2x + 3 = 9");

            // now let's ask it a question'

            var respuesta = await GenerarRespuesta(ejercicio, chat, idioma);
            return (respuesta);
        }

        public async Task<EjercicioAdaptadoDTO> SimplificarTexto (EjercicioPorAdaptarDTO ejercicio, string profesion, int edad, string idioma)
        {
            // api instance
            var api = new OpenAI_API.OpenAIAPI(_openAIConfiguration.Key);
            var chat = api.Chat.CreateConversation();

            /// give instruction as System
            var system = "Sos un " + profesion + " de niños de " + edad + " años. Tenes alumnos con dislexia. Tenes que adaptar ejercicios para ellos. La adaptacion tiene que consistir en simplificar el ejercicio a lo mas escencial para poder seguir respondiendo a las preguntas. Respetar el idioma del ejercicio. No tenes que modificar ningun contenido de nada. Y tampoco tenes que agregar palabaras como Consigna, Ejercicio, Texto, etc. Solo responde con la adaptacion";
            chat.AppendSystemMessage(system);

            // give a few examples as user and assistant
            chat.AppendUserInput("Adapta la siguiente consigna: Lea el texto y responda las preguntas:");
            chat.AppendExampleChatbotOutput("Lea el texto y responda las preguntas:");
            chat.AppendUserInput("Adapta el siguiente texto en base a la consigna: Lea el texto y responda las preguntas:. El ejercicio es: ¿Dónde vivía Pedro? ¿Qué encontró Pedro mientras caminaba cerca del río ? ¿Qué conexión tenía Pedro con la naturaleza ? ¿Qué hizo Pedro después de encontrar el medallón?. El texto es el siguiente: En un tranquilo pueblo rodeado de montañas y bosques, vivía un joven llamado Pedro. Pedro siempre había sentido una fuerte conexión con la naturaleza y pasaba horas explorando los senderos del bosque. Un día, mientras caminaba cerca de un río, encontró un extraño objeto brillante en el suelo. Era un medallón con inscripciones antiguas. Intrigado, Pedro decidió investigar su significado y descubrió que el medallón estaba relacionado con una antigua leyenda sobre un tesoro escondido en las profundidades del bosque. A partir de ese momento, Pedro se embarcó en una emocionante búsqueda llena de desafíos y aventuras.");
            chat.AppendExampleChatbotOutput("Pedro vivía en un pueblo rodeado de montañas. Un día, encontró un medallón brillante cerca de un río. Le gustaba explorar la naturaleza. Decidió investigar el medallón y descubrió una leyenda sobre un tesoro. Comenzó una búsqueda emocionante.");
            chat.AppendUserInput("Adapta el siguiente ejercicio en base a la consigna: Lea el texto y responda las preguntas:. El ejercicio es el siguiente: ¿Dónde vivía Pedro? ¿Qué encontró Pedro mientras caminaba cerca del río ? ¿Qué conexión tenía Pedro con la naturaleza ? ¿Qué hizo Pedro después de encontrar el medallón?");
            chat.AppendExampleChatbotOutput("1)¿Dónde vivía Pedro? \n 2)¿Qué encontró Pedro mientras caminaba cerca del río ? \n 3)¿Qué conexión tenía Pedro con la naturaleza ? \n 4)¿Qué hizo Pedro después de encontrar el medallón?");

            // now let's ask it a question'

            var respuesta = await GenerarRespuesta(ejercicio, chat, idioma);
            return (respuesta);
        }

        public async Task<EjercicioAdaptadoDTO> GenerarRespuesta(EjercicioPorAdaptarDTO ejercicio, OpenAI_API.Chat.Conversation chat, string idioma)
        {
            var respuesta = new EjercicioAdaptadoDTO();
            var pregunta = "El idioma es " + idioma + ".Adapta la siguiente consigna: " + ejercicio.Consigna;
            chat.AppendUserInput(pregunta);
            // and get the response
            string response = await chat.GetResponseFromChatbotAsync();
            Console.WriteLine(response);
            respuesta.Consigna = response;

            if (ejercicio.Texto != "")
            {
                pregunta = "El idioma es "+ idioma +".Adapta el siguiente texto en base a la consigna: " + ejercicio.Consigna + ". No incluyas la consigna en la respuesta. Y en base al siguiente ejercicio: "+ ejercicio.Ejercicio + ". No incluyas el ejercicio en la respuesta. El texto es el siguiente: " + ejercicio.Texto;
                chat.AppendUserInput(pregunta);
                // and get the response
                response = await chat.GetResponseFromChatbotAsync();
                Console.WriteLine(response);
                respuesta.Ejercicio = response;
                pregunta = "El idioma es " + idioma + ".Adapta el siguiente ejercicio en base a la consigna: " + ejercicio.Consigna + ejercicio.Texto + ". El ejercicio es el siguiente: " + ejercicio.Ejercicio;
                chat.AppendUserInput(pregunta);
                // and get the response
                response = await chat.GetResponseFromChatbotAsync();
                Console.WriteLine(response);
                respuesta.Ejercicio = respuesta.Ejercicio + "\n" + response;
            }
            else
            {
                pregunta = "El idioma es " + idioma + ".Adapta el siguiente ejercicio en base a la consigna: " + ejercicio.Consigna + ". El ejercicio es el siguiente: " + ejercicio.Ejercicio;
                chat.AppendUserInput(pregunta);
                // and get the response
                response = await chat.GetResponseFromChatbotAsync();
                Console.WriteLine(response);
                respuesta.Ejercicio = response;
            }
            return respuesta;
        }

    }
}

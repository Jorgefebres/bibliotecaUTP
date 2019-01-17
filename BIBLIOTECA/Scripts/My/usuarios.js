function IniciarSesion() { $("#modal-content").load("/Usuario/Login"); };

function Registrate() { $("#modal-content").load("/Usuario/Create"); };


function EliminarID(id) { $("#contentModal").load("/Usuario/Delete/" + id); };

function Editar(id) { $("#contenidoModal").load("/Usuario/EditAdmin/" + id); };

function EditarUsuario(id) { $("#contenidoModal").load("/Usuario/Edit/" + id); };

function EditarPass(id) { $("#contenidoModal").load("/Usuario/EditPass/" + id); };
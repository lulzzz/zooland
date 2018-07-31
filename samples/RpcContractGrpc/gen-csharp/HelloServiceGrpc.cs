// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: helloService.proto
#pragma warning disable 1591
#region Designer generated code

using System;
using System.Threading;
using System.Threading.Tasks;
using grpc = global::Grpc.Core;

namespace RpcContractGrpc {
  /// <summary>
  /// The hello service definition.
  /// </summary>
  public static partial class HelloService
  {
    static readonly string __ServiceName = "RpcContractGrpc.HelloService";

    static readonly grpc::Marshaller<global::RpcContractGrpc.Void> __Marshaller_Void = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::RpcContractGrpc.Void.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::RpcContractGrpc.NameResult> __Marshaller_NameResult = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::RpcContractGrpc.NameResult.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::RpcContractGrpc.HelloResult> __Marshaller_HelloResult = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::RpcContractGrpc.HelloResult.Parser.ParseFrom);

    static readonly grpc::Method<global::RpcContractGrpc.Void, global::RpcContractGrpc.NameResult> __Method_CallNameVoid = new grpc::Method<global::RpcContractGrpc.Void, global::RpcContractGrpc.NameResult>(
        grpc::MethodType.Unary,
        __ServiceName,
        "CallNameVoid",
        __Marshaller_Void,
        __Marshaller_NameResult);

    static readonly grpc::Method<global::RpcContractGrpc.NameResult, global::RpcContractGrpc.Void> __Method_CallName = new grpc::Method<global::RpcContractGrpc.NameResult, global::RpcContractGrpc.Void>(
        grpc::MethodType.Unary,
        __ServiceName,
        "CallName",
        __Marshaller_NameResult,
        __Marshaller_Void);

    static readonly grpc::Method<global::RpcContractGrpc.Void, global::RpcContractGrpc.Void> __Method_CallVoid = new grpc::Method<global::RpcContractGrpc.Void, global::RpcContractGrpc.Void>(
        grpc::MethodType.Unary,
        __ServiceName,
        "CallVoid",
        __Marshaller_Void,
        __Marshaller_Void);

    static readonly grpc::Method<global::RpcContractGrpc.NameResult, global::RpcContractGrpc.NameResult> __Method_Hello = new grpc::Method<global::RpcContractGrpc.NameResult, global::RpcContractGrpc.NameResult>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Hello",
        __Marshaller_NameResult,
        __Marshaller_NameResult);

    static readonly grpc::Method<global::RpcContractGrpc.NameResult, global::RpcContractGrpc.HelloResult> __Method_SayHello = new grpc::Method<global::RpcContractGrpc.NameResult, global::RpcContractGrpc.HelloResult>(
        grpc::MethodType.Unary,
        __ServiceName,
        "SayHello",
        __Marshaller_NameResult,
        __Marshaller_HelloResult);

    static readonly grpc::Method<global::RpcContractGrpc.HelloResult, global::RpcContractGrpc.NameResult> __Method_ShowHello = new grpc::Method<global::RpcContractGrpc.HelloResult, global::RpcContractGrpc.NameResult>(
        grpc::MethodType.Unary,
        __ServiceName,
        "ShowHello",
        __Marshaller_HelloResult,
        __Marshaller_NameResult);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::RpcContractGrpc.HelloServiceReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of HelloService</summary>
    public abstract partial class HelloServiceBase
    {
      public virtual global::System.Threading.Tasks.Task<global::RpcContractGrpc.NameResult> CallNameVoid(global::RpcContractGrpc.Void request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::RpcContractGrpc.Void> CallName(global::RpcContractGrpc.NameResult request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::RpcContractGrpc.Void> CallVoid(global::RpcContractGrpc.Void request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      /// <summary>
      /// Hello
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      public virtual global::System.Threading.Tasks.Task<global::RpcContractGrpc.NameResult> Hello(global::RpcContractGrpc.NameResult request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      /// <summary>
      /// SayHello
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      public virtual global::System.Threading.Tasks.Task<global::RpcContractGrpc.HelloResult> SayHello(global::RpcContractGrpc.NameResult request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      /// <summary>
      /// ShowHello
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      public virtual global::System.Threading.Tasks.Task<global::RpcContractGrpc.NameResult> ShowHello(global::RpcContractGrpc.HelloResult request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for HelloService</summary>
    public partial class HelloServiceClient : grpc::ClientBase<HelloServiceClient>
    {
      /// <summary>Creates a new client for HelloService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public HelloServiceClient(grpc::Channel channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for HelloService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public HelloServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected HelloServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected HelloServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::RpcContractGrpc.NameResult CallNameVoid(global::RpcContractGrpc.Void request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return CallNameVoid(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::RpcContractGrpc.NameResult CallNameVoid(global::RpcContractGrpc.Void request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_CallNameVoid, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::RpcContractGrpc.NameResult> CallNameVoidAsync(global::RpcContractGrpc.Void request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return CallNameVoidAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::RpcContractGrpc.NameResult> CallNameVoidAsync(global::RpcContractGrpc.Void request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_CallNameVoid, null, options, request);
      }
      public virtual global::RpcContractGrpc.Void CallName(global::RpcContractGrpc.NameResult request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return CallName(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::RpcContractGrpc.Void CallName(global::RpcContractGrpc.NameResult request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_CallName, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::RpcContractGrpc.Void> CallNameAsync(global::RpcContractGrpc.NameResult request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return CallNameAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::RpcContractGrpc.Void> CallNameAsync(global::RpcContractGrpc.NameResult request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_CallName, null, options, request);
      }
      public virtual global::RpcContractGrpc.Void CallVoid(global::RpcContractGrpc.Void request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return CallVoid(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::RpcContractGrpc.Void CallVoid(global::RpcContractGrpc.Void request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_CallVoid, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::RpcContractGrpc.Void> CallVoidAsync(global::RpcContractGrpc.Void request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return CallVoidAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::RpcContractGrpc.Void> CallVoidAsync(global::RpcContractGrpc.Void request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_CallVoid, null, options, request);
      }
      /// <summary>
      /// Hello
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::RpcContractGrpc.NameResult Hello(global::RpcContractGrpc.NameResult request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return Hello(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Hello
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::RpcContractGrpc.NameResult Hello(global::RpcContractGrpc.NameResult request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Hello, null, options, request);
      }
      /// <summary>
      /// Hello
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::RpcContractGrpc.NameResult> HelloAsync(global::RpcContractGrpc.NameResult request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return HelloAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Hello
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::RpcContractGrpc.NameResult> HelloAsync(global::RpcContractGrpc.NameResult request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Hello, null, options, request);
      }
      /// <summary>
      /// SayHello
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::RpcContractGrpc.HelloResult SayHello(global::RpcContractGrpc.NameResult request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return SayHello(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// SayHello
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::RpcContractGrpc.HelloResult SayHello(global::RpcContractGrpc.NameResult request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_SayHello, null, options, request);
      }
      /// <summary>
      /// SayHello
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::RpcContractGrpc.HelloResult> SayHelloAsync(global::RpcContractGrpc.NameResult request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return SayHelloAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// SayHello
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::RpcContractGrpc.HelloResult> SayHelloAsync(global::RpcContractGrpc.NameResult request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_SayHello, null, options, request);
      }
      /// <summary>
      /// ShowHello
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::RpcContractGrpc.NameResult ShowHello(global::RpcContractGrpc.HelloResult request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return ShowHello(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// ShowHello
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::RpcContractGrpc.NameResult ShowHello(global::RpcContractGrpc.HelloResult request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_ShowHello, null, options, request);
      }
      /// <summary>
      /// ShowHello
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::RpcContractGrpc.NameResult> ShowHelloAsync(global::RpcContractGrpc.HelloResult request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return ShowHelloAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// ShowHello
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::RpcContractGrpc.NameResult> ShowHelloAsync(global::RpcContractGrpc.HelloResult request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_ShowHello, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override HelloServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new HelloServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(HelloServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_CallNameVoid, serviceImpl.CallNameVoid)
          .AddMethod(__Method_CallName, serviceImpl.CallName)
          .AddMethod(__Method_CallVoid, serviceImpl.CallVoid)
          .AddMethod(__Method_Hello, serviceImpl.Hello)
          .AddMethod(__Method_SayHello, serviceImpl.SayHello)
          .AddMethod(__Method_ShowHello, serviceImpl.ShowHello).Build();
    }

  }
}
#endregion